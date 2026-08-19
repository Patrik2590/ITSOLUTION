using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITSOLUTION.Infrastructure.Persistence;

namespace ITSOLUTION.WebAPI.Controllers
{
    [Authorize] // 🔒 Esta simple etiqueta bloquea a cualquiera que no tenga un Token válido
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmpleadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpleados()
        {
            // 🪄 LA MAGIA DEL ERP: 
            // Fíjate que NO hay ningún "Where(e => e.TenantId == ...)".
            // Gracias a tu CurrentUserService y al filtro global, Entity Framework 
            // leerá el Token y hará el filtrado automáticamente en SQL Server.
            var empleados = await _context.Empleados.ToListAsync();

            return Ok(empleados);
        }
    }
}