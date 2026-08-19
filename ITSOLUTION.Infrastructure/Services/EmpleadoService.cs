using ITSOLUTION.Application.DTOs;
using ITSOLUTION.Domain.Entities;
using ITSOLUTION.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITSOLUTION.Infrastructure.Services
{
    public class EmpleadoService
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener la lista de empleados
        public async Task<List<EmpleadoDto>> ObtenerTodosAsync()
        {
            return await _context.Empleados
                .Select(e => new EmpleadoDto
                {
                    Id = e.Id,
                    NombreCompleto = e.NombreCompleto,
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
                NombreCompleto = dto.NombreCompleto,
                Departamento = dto.Departamento,
                Puesto = dto.Puesto,
                CorreoCorporativo = dto.CorreoCorporativo,
                EstaActivo = true // Por defecto, al crearlo está activo
            };

            _context.Empleados.Add(nuevoEmpleado);
            await _context.SaveChangesAsync();

            return new EmpleadoDto
            {
                Id = nuevoEmpleado.Id,
                NombreCompleto = nuevoEmpleado.NombreCompleto,
                Departamento = nuevoEmpleado.Departamento,
                Puesto = nuevoEmpleado.Puesto,
                CorreoCorporativo = nuevoEmpleado.CorreoCorporativo,
                EstaActivo = nuevoEmpleado.EstaActivo
            };
        }
    }
}