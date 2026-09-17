using ITSOLUTION.Application.DTOs;
using ITSOLUTION.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSOLUTION.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Descomenta cuando vayas a probar con JWT
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly IValidator<TenantDto> _validator;

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
            var validationResult = await _validator.ValidateAsync(tenantDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var created = await _tenantService.CreateTenantAsync(tenantDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // --- ENDPOINT PARA ESTRUCTURA COMPLETA CON INNER EXCEPTION CAPTURADA ---
        [HttpPost("Complete")]
        public async Task<ActionResult<TenantDto>> CreateComplete([FromBody] CreateTenantCompleteDto request)
        {
            try
            {
                var created = await _tenantService.CreateTenantWithDetailsAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                // Extrae el mensaje interno real de SQL Server / Entity Framework
                var errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return StatusCode(500, new
                {
                    message = "Ocurrió un error al guardar la empresa.",
                    details = errorReal
                });
            }
        }

        [HttpPut("Complete/{id}")]
        public async Task<ActionResult<TenantDto>> UpdateComplete(Guid id, [FromBody] CreateTenantCompleteDto request)
        {
            try
            {
                var updatedTenant = await _tenantService.UpdateTenantWithDetailsAsync(id, request);
                return Ok(updatedTenant);
            }
            catch (Exception ex)
            {
                // Obtenemos el mensaje de error real para mostrarlo en el frontend
                var errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return StatusCode(500, new
                {
                    message = "Ocurrió un error al actualizar la empresa.",
                    details = errorReal
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<TenantDto>> Update(Guid id, [FromBody] TenantDto tenantDto)
        {
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

            return Ok(new { message = "Empresa desactivada correctamente." });
        }
    }
}