using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITSOLUTION.Application.DTOs.Activos;
using ITSOLUTION.Domain.Entities;
using ITSOLUTION.Infrastructure.Persistence;

namespace ITSOLUTION.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // 🚀 Aquí aplicamos el "Constructor Principal" sugerido por Visual Studio
    public class ActivosController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivoDto>>> GetActivos()
        {
            var activos = await context.Activos // Nota que ahora usamos 'context' directo
                .AsNoTracking()
                .Select(a => new ActivoDto
                {
                    Id = a.Id,
                    CodigoInventario = a.CodigoInventario,
                    Categoria = a.Categoria,
                    Marca = a.Marca,
                    Modelo = a.Modelo,
                    NumeroSerie = a.NumeroSerie, 
                    Estado = a.Estado,
                    Ubicacion = a.Ubicacion,
                    EmpleadoId = a.EmpleadoId,
                    // ⚠️ OJO AQUÍ: Cambié a 'Nombre' y 'Apellido' en singular. 
                    EmpleadoNombre = a.Empleado != null ? a.Empleado.Nombres + " " + a.Empleado.Apellidos : "Sin Asignar (Bodega)",
                    Procesador = a.Procesador,
                    MemoriaRam = a.MemoriaRam,
                    Almacenamiento = a.Almacenamiento,
                    DireccionIP = a.DireccionIP,
                    DireccionMAC = a.DireccionMAC
                })
                .ToListAsync();

            return Ok(activos);
        }

        [HttpPost]
        public async Task<ActionResult<ActivoDto>> CreateActivo(CreateActivoDto dto)
        {
            var nuevoActivo = new ActivoIT
            {
                CodigoInventario = dto.CodigoInventario,
                Categoria = dto.Categoria,
                TipoEquipo = dto.TipoEquipo,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                NumeroSerie = dto.NumeroSerie,
                Estado = dto.Estado,
                Ubicacion = dto.Ubicacion,
                Observaciones = dto.Observaciones,
                EmpleadoId = dto.EmpleadoId,
                Procesador = dto.Procesador,
                MemoriaRam = dto.MemoriaRam,
                Almacenamiento = dto.Almacenamiento,
                DireccionIP = dto.DireccionIP,
                DireccionMAC = dto.DireccionMAC
            };

            context.Activos.Add(nuevoActivo);
            await context.SaveChangesAsync();

            return Ok(new { mensaje = "Activo registrado correctamente", id = nuevoActivo.Id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivo(int id)
        {
            var activo = await context.Activos.FindAsync(id);
            if (activo == null)
            {
                return NotFound(new { mensaje = "El activo no existe" });
            }

            context.Activos.Remove(activo);
            await context.SaveChangesAsync();

            return Ok(new { mensaje = "Activo eliminado correctamente" });
        }
    }
}