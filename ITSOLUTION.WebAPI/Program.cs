using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ITSOLUTION.Application.Interfaces;
using ITSOLUTION.Infrastructure.Persistence;
using ITSOLUTION.Infrastructure.Services;
using ITSOLUTION.WebAPI.Services;
// Nota: Si tienes un EmpleadoService o TenantService en otras carpetas, 
// asegúrate de que sus "usings" estén aquí arriba.

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURACIÓN DE LA BASE DE DATOS
// ==========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
    )
);

// ==========================================
// 2. INYECCIÓN DE DEPENDENCIAS (SERVICIOS)
// ==========================================
// Lector de contexto HTTP (Vital para leer el Token en tiempo real)
builder.Services.AddHttpContextAccessor();

// Servicios de Infraestructura (Seguridad Multi-Tenant)
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Servicios de Negocio (Asegúrate de que coincidan con tus clases reales)
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<EmpleadoService>();

// Validadores (FluentValidation)
builder.Services.AddScoped<FluentValidation.IValidator<ITSOLUTION.Application.DTOs.TenantDto>, ITSOLUTION.Application.Validations.TenantDtoValidator>();

builder.Services.AddControllers();

// ==========================================
// 2.5 CONFIGURACIÓN DE CORS (Para conectar con Angular)
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Asegúrate de que este es el puerto de tu Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ==========================================
// 3. CONFIGURACIÓN DE SWAGGER (CON SEGURIDAD JWT)
// ==========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ITSOLUTION ERP API", Version = "v1" });

    // Definir el esquema de seguridad (El candado visual en Swagger)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT. Escribe 'Bearer' [espacio] y luego tu token.\nEjemplo: 'Bearer eyJhbGci...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Exigir el token globalmente en la interfaz
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// ==========================================
// 4. CONFIGURACIÓN DE AUTENTICACIÓN JWT
// ==========================================
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("La clave Jwt:Key no existe en el appsettings.json");
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Evita tiempos de gracia adicionales en la expiración
        };
    });

var app = builder.Build();

// ==========================================
// 5. PIPELINE HTTP (MIDDLEWARES)
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // 👈 Permite acceder a los archivos de la carpeta wwwroot

// 🌐 APLICAR CORS AQUÍ (Antes de la Autenticación es obligatorio)
app.UseCors("AllowAngular");

// 🛡️ EL ORDEN ES VITAL AQUÍ
app.UseAuthentication(); // 1º Autenticar (Identificar al usuario por su Token)
app.UseAuthorization();  // 2º Autorizar (Validar qué tiene permitido hacer)

app.MapControllers();

app.Run();