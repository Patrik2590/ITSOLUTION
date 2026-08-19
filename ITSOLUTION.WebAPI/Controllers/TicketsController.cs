using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting; // 👈 Necesario para IWebHostEnvironment
using ITSOLUTION.Infrastructure.Persistence;
using ITSOLUTION.Domain.Entities;
using ITSOLUTION.Application.Interfaces;

namespace ITSOLUTION.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔒 CRÍTICO: Obliga a usar el Token JWT en todas las peticiones
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWebHostEnvironment _env; // 👈 Entorno para guardar archivos correctamente

        public TicketsController(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IWebHostEnvironment env)
        {
            _context = context;
            _currentUserService = currentUserService;
            _env = env;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            // 🛡️ El Global Query Filter del DbContext filtra automáticamente por TenantId y SucursalId
            var tickets = await _context.Tickets
                .OrderByDescending(t => t.FechaReporte)
                .ToListAsync();

            return Ok(tickets);
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] Ticket newTicket)
        {
            // 🔒 Asignamos automáticamente las credenciales de seguridad por detrás
            newTicket.TenantId = _currentUserService.TenantId;
            newTicket.SucursalId = _currentUserService.SucursalId;

            // ⏱️ Asignaciones automáticas de negocio
            newTicket.FechaReporte = DateTime.UtcNow;
            newTicket.Status = "Abierto";

            // 👨‍🔧 Los campos del técnico nacen obligatoriamente vacíos
            newTicket.HoraAtencion = null;
            newTicket.HoraCierre = null;
            newTicket.ActividadesRealizadas = string.Empty;
            newTicket.Observaciones = string.Empty;

            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();

            return Ok(newTicket);
        }

        // PUT: api/Tickets/{id}/Atender
        [HttpPut("{id}/Atender")]
        public async Task<IActionResult> AtenderTicket(int id)
        {
            // 🧹 Clean Code: FindAsync ya aplica los filtros globales de seguridad de Entity Framework
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null) return NotFound("Ticket no encontrado o sin acceso.");

            if (ticket.Status != "Abierto")
                return BadRequest("El ticket ya fue atendido o está cerrado.");

            // Registramos la hora exacta
            ticket.Status = "EnProceso";
            ticket.HoraAtencion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        // PUT: api/Tickets/{id}/Cerrar
        [HttpPut("{id}/Cerrar")]
        public async Task<IActionResult> CerrarTicket(int id, [FromBody] Ticket request)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null) return NotFound("Ticket no encontrado o sin acceso.");

            if (ticket.Status == "Cerrado")
                return BadRequest("El ticket ya se encuentra cerrado.");

            // Guardamos la evidencia del trabajo
            ticket.ActividadesRealizadas = request.ActividadesRealizadas;
            ticket.Observaciones = request.Observaciones;

            // Sellamos el ticket
            ticket.Status = "Cerrado";
            ticket.HoraCierre = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        // POST: api/Tickets/{id}/Upload
        [HttpPost("{id}/Upload")]
        public async Task<IActionResult> UploadAttachment(int id, IFormFile file)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null) return NotFound("Ticket no encontrado.");
            if (file == null || file.Length == 0) return BadRequest("No se envió ningún archivo.");

            // 📂 Forma correcta de manejar rutas en .NET Core
            // Si _env.WebRootPath es null, nos aseguramos de usar la ruta base + wwwroot
            var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(webRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generamos un nombre único para el archivo
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Guardamos el archivo físicamente en el disco duro
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Guardamos el registro en la base de datos
            var attachment = new TicketAttachment
            {
                TicketId = ticket.Id,
                FileName = file.FileName,
                FilePath = $"/uploads/{uniqueFileName}", // URL que consumirá Angular
                ContentType = file.ContentType
            };

            _context.TicketAttachments.Add(attachment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = attachment.Id,
                fileName = attachment.FileName,
                filePath = attachment.FilePath,
                contentType = attachment.ContentType
            });
        }
    }
}