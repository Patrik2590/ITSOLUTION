using ITSOLUTION.Application.DTOs;
using ITSOLUTION.Domain.Entities;
using ITSOLUTION.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITSOLUTION.Infrastructure.Services
{
    // 🚀 Aplicamos el constructor principal (Primary Constructor) para un código más limpio
    public class EmpleadoService(ApplicationDbContext context)
    {
        // Obtener la lista de empleados
        public async Task<List<EmpleadoDto>> ObtenerTodosAsync()
        {
            return await context.Empleados
                .Select(e => new EmpleadoDto
                {
                    Id = e.Id,
                    // 🚀 Reemplazamos NombreCompleto por los nuevos campos
                    Cedula = e.Cedula,
                    Nombres = e.Nombres,
                    Apellidos = e.Apellidos,

                    Departamento = e.Departamento,
                    Puesto = e.Puesto,
                    CorreoCorporativo = e.CorreoCorporativo,
                    EstaActivo = e.EstaActivo
                }).ToListAsync();
        }

        // Registrar un nuevo empleado
        public async Task<EmpleadoDto> CrearAsync(CreateEmpleadoDto dto)
        {
            var nuevoEmpleado = new Empleado
            {
                // 🚀 Asignamos los nuevos campos que vienen desde el DTO
                Cedula = dto.Cedula,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,

                Departamento = dto.Departamento,
                Puesto = dto.Puesto,
                CorreoCorporativo = dto.CorreoCorporativo,
                EstaActivo = true // Por defecto, al crearlo está activo
            };

            context.Empleados.Add(nuevoEmpleado);
            await context.SaveChangesAsync();

            return new EmpleadoDto
            {
                Id = nuevoEmpleado.Id,
                // 🚀 Devolvemos los nuevos campos al frontend
                Cedula = nuevoEmpleado.Cedula,
                Nombres = nuevoEmpleado.Nombres,
                Apellidos = nuevoEmpleado.Apellidos,

                Departamento = nuevoEmpleado.Departamento,
                Puesto = nuevoEmpleado.Puesto,
                CorreoCorporativo = nuevoEmpleado.CorreoCorporativo,
                EstaActivo = nuevoEmpleado.EstaActivo
            };
        }
    }
}