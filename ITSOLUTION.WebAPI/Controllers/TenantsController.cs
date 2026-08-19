using ITSOLUTION.Application.DTOs;
using ITSOLUTION.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ITSOLUTION.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly IValidator<TenantDto> _validator;

        // Inyectamos el servicio y el validador
        public TenantsController(ITenantService tenantService, IValidator<TenantDto> validator)
        {
            _tenantService = tenantService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TenantDto>>> GetAll()
        {
            return Ok(await _tenantService.GetAllTenantsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TenantDto>> GetById(Guid id)
        {
            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant == null) return NotFound("Empresa no encontrada.");
            return Ok(tenant);
        }

        [HttpPost]
        public async Task<ActionResult<TenantDto>> Create([FromBody] TenantDto tenantDto)
        {
            // 1. Ejecutar las reglas de validación
            var validationResult = await _validator.ValidateAsync(tenantDto);
            if (!validationResult.IsValid)
            {
                // Si falla, devolvemos un Error 400 con la lista de mensajes
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var created = await _tenantService.CreateTenantAsync(tenantDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TenantDto>> Update(Guid id, [FromBody] TenantDto tenantDto)
        {
            // Validamos también al actualizar
            var validationResult = await _validator.ValidateAsync(tenantDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var updated = await _tenantService.UpdateTenantAsync(id, tenantDto);
            if (updated == null) return NotFound("Empresa no encontrada.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deactivate(Guid id)
        {
            var success = await _tenantService.DeactivateTenantAsync(id);
            if (!success) return NotFound("Empresa no encontrada.");

            return Ok("Empresa desactivada correctamente.");
        }
    }
}