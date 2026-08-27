using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ITSOLUTION.Infrastructure.Persistence;
using ITSOLUTION.Application.DTOs.Auth;
using Microsoft.EntityFrameworkCore;

namespace ITSOLUTION.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // 1. Buscamos al usuario e INCLUIMOS sus nuevos roles relacionales (Motor RBAC)
            var usuario = await _context.Usuarios
                .IgnoreQueryFilters()
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Correo == request.Correo && u.Password == request.Password);

            if (usuario == null)
                return Unauthorized(new { message = "Credenciales incorrectas." });

            if (!usuario.EstaActivo)
                return Unauthorized(new { message = "El usuario está inactivo. Contacte al administrador." });

            // =========================================================
            // 🛡️ NUEVO: VALIDACIÓN DE ROLES (Evita el Error 500)
            // =========================================================
            if (usuario.UsuariosRoles == null || !usuario.UsuariosRoles.Any())
            {
                // Retornamos 403 Forbidden para que Angular lance la alerta amarilla
                return StatusCode(403, new { message = "Tu cuenta existe, pero no tienes permisos asignados. Contacta al administrador." });
            }

            // 2. Construcción de los Claims básicos
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                
                // ¡AQUÍ ESTÁ LA MAGIA DEL ERP!
                new Claim("TenantId", usuario.TenantId.ToString()),
                new Claim("SucursalId", usuario.SucursalId.ToString())
            };

            // 3. Iteramos sobre los roles dinámicos de la base de datos
            // (Ya no necesitamos comprobar si es null porque lo bloqueamos arriba)
            foreach (var ur in usuario.UsuariosRoles)
            {
                if (ur.Rol != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, ur.Rol.Nombre));
                }
            }

            // 4. Generamos el Token
            var keyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Devolvemos la respuesta a Angular
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                nombre = usuario.NombreCompleto,
                // Optimizamos esto: ya sabemos que la lista de roles tiene datos
                roles = usuario.UsuariosRoles.Select(ur => ur.Rol.Nombre).ToList()
            });
        }
    }
}