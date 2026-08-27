using System;
using System.Collections.Generic;

namespace ITSOLUTION.Application.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Solicita { get; set; } = string.Empty;

        // Tiempos
        public DateTime FechaReporte { get; set; }
        public DateTime? HoraAtencion { get; set; }
        public DateTime? HoraCierre { get; set; }

        // Trabajo del técnico
        public string ActividadesRealizadas { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;

        // Metadatos
        public string Status { get; set; } = string.Empty;

        // Relación plana sin dependencias circulares
        public List<TicketAttachmentDto> Attachments { get; set; } = new();
    }
}