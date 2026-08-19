using ITSOLUTION.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ITSOLUTION.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        // GET: api/dashboard
        [HttpGet]
        public ActionResult<DashboardDto> GetDashboardSummary()
        {
            // Por ahora enviaremos datos "quemados" (hardcoded) para conectar con Angular.
            // Más adelante conectaremos esto con tu infraestructura y repositorios.

            var dashboardData = new DashboardDto
            {
                Kpis = new KpiDto
                {
                    SystemUptime = "99.98%",
                    ActiveTickets = 42,
                    SecurityAlerts = 2,
                    PendingUpdates = 8
                },
                RecentAlerts = new List<AlertDto>
                {
                    new AlertDto { Id = "TKT-2055", Type = "Warning", Message = "CPU usage at 85% on Server-04", Time = "2 mins ago" },
                    new AlertDto { Id = "SEC-901", Type = "Info", Message = "Firewall rules updated successfully", Time = "15 mins ago" },
                    new AlertDto { Id = "TKT-2056", Type = "Critical", Message = "Database connection timeout", Time = "1 hour ago" }
                }
            };

            return Ok(dashboardData);
        }
    }
}