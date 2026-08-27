using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using ITSOLUTION.Infrastructure.Persistence;
using ITSOLUTION.Domain.Entities;
using ITSOLUTION.Application.Interfaces;
using ITSOLUTION.Application.DTOs; // 👈 Importamos los DTOs
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ITSOLUTION.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔒 Obliga a usar el Token JWT
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWebHostEnvironment _env;

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
        [HttpGet] // 🧹 Eliminado el decorador duplicado
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Attachments)
                .OrderByDescending(t => t.FechaReporte)
                .Select(t => new TicketDto // 👈 Magia pura: Transformamos al DTO
                {
                    Id = t.Id,
                    NombreUsuario = t.NombreUsuario,
                    Area = t.Area,
                    Solicita = t.Solicita,
                    FechaReporte = t.FechaReporte,
                    HoraAtencion = t.HoraAtencion,
                    HoraCierre = t.HoraCierre,
                    ActividadesRealizadas = t.ActividadesRealizadas,
                    Observaciones = t.Observaciones,
                    Status = t.Status,
                    Attachments = t.Attachments.Select(a => new TicketAttachmentDto
                    {
                        Id = a.Id,
                        FileName = a.FileName,
                        FilePath = a.FilePath,
                        ContentType = a.ContentType
                    }).ToList()
                })
                .ToListAsync();

            return Ok(tickets);
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] Ticket newTicket)
        {
            newTicket.TenantId = _currentUserService.TenantId;
            newTicket.SucursalId = _currentUserService.SucursalId;
            newTicket.FechaReporte = DateTime.UtcNow;
            newTicket.Status = "Abierto";
            newTicket.HoraAtencion = null;
            newTicket.HoraCierre = null;
            newTicket.ActividadesRealizadas = string.Empty;
            newTicket.Observaciones = string.Empty;

            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();

            // Retornamos el DTO seguro
            return Ok(MapToDto(newTicket));
        }

        // PUT: api/Tickets/{id}/Atender
        [HttpPut("{id}/Atender")]
        public async Task<IActionResult> AtenderTicket(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return NotFound("Ticket no encontrado o sin acceso.");
            if (ticket.Status != "Abierto") return BadRequest("El ticket ya fue atendido o está cerrado.");

            ticket.Status = "EnProceso";
            ticket.HoraAtencion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(MapToDto(ticket));
        }

        // PUT: api/Tickets/{id}/Cerrar
        [HttpPut("{id}/Cerrar")]
        public async Task<IActionResult> CerrarTicket(int id, [FromBody] Ticket request)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return NotFound("Ticket no encontrado o sin acceso.");
            if (ticket.Status == "Cerrado") return BadRequest("El ticket ya se encuentra cerrado.");

            ticket.ActividadesRealizadas = request.ActividadesRealizadas;
            ticket.Observaciones = request.Observaciones;
            ticket.Status = "Cerrado";
            ticket.HoraCierre = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(MapToDto(ticket));
        }

        // POST: api/Tickets/{id}/Upload
        [HttpPost("{id}/Upload")]
        public async Task<IActionResult> UploadAttachment(int id, IFormFile file)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null) return NotFound("Ticket no encontrado.");
            if (file == null || file.Length == 0) return BadRequest("No se envió ningún archivo.");

            var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(webRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new TicketAttachment
            {
                TicketId = ticket.Id,
                FileName = file.FileName,
                FilePath = $"/uploads/{uniqueFileName}",
                ContentType = file.ContentType
            };

            _context.TicketAttachments.Add(attachment);
            await _context.SaveChangesAsync();

            // Devolvemos el DTO plano
            return Ok(new TicketAttachmentDto
            {
                Id = attachment.Id,
                FileName = attachment.FileName,
                FilePath = attachment.FilePath,
                ContentType = attachment.ContentType
            });
        }

        // 🛠️ MÉTODO PRIVADO DE APOYO (Clean Code)
        // Mapea la entidad hacia el DTO para no repetir código en el POST y los PUT
        private TicketDto MapToDto(Ticket t)
        {
            return new TicketDto
            {
                Id = t.Id,
                NombreUsuario = t.NombreUsuario,
                Area = t.Area,
                Solicita = t.Solicita,
                FechaReporte = t.FechaReporte,
                HoraAtencion = t.HoraAtencion,
                HoraCierre = t.HoraCierre,
                ActividadesRealizadas = t.ActividadesRealizadas,
                Observaciones = t.Observaciones,
                Status = t.Status,
                Attachments = t.Attachments?.Select(a => new TicketAttachmentDto
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FilePath = a.FilePath,
                    ContentType = a.ContentType
                }).ToList() ?? new List<TicketAttachmentDto>()
            };
        }
    }
}