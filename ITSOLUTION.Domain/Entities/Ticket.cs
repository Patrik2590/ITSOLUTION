using System;

namespace ITSOLUTION.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        // --- 1. SECCIÓN DEL USUARIO ---
        public string NombreUsuario { get; set; } = string.Empty; // Ej: Ivonne Tulcanaza
        public string Area { get; set; } = string.Empty;          // Ej: Coordinación
        public string Solicita { get; set; } = string.Empty;      // Descripción del problema

        // --- 2. SECCIÓN DE TIEMPOS ---
        public DateTime FechaReporte { get; set; } = DateTime.UtcNow;
        public DateTime? HoraAtencion { get; set; }
        public DateTime? HoraCierre { get; set; }

        // --- 3. SECCIÓN DEL TÉCNICO ---
        public string ActividadesRealizadas { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;

        // --- 4. METADATOS DEL SISTEMA ---
        public string Status { get; set; } = "Abierto"; // Abierto, EnProceso, Cerrado
        public string Priority { get; set; } = "Normal";

        // --- 5. SEGURIDAD MULTI-TENANT ---
        public Guid TenantId { get; set; }
        public int SucursalId { get; set; }

        // --- 6. ARCHIVOS ADJUNTOS (Evidencias) ---
        public ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
    }

}