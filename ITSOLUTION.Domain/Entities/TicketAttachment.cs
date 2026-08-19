using System;

namespace ITSOLUTION.Domain.Entities
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }

        public string FileName { get; set; } = string.Empty; // Nombre original del archivo
        public string FilePath { get; set; } = string.Empty; // Ruta donde se guardó (Ej: /uploads/img123.jpg)
        public string ContentType { get; set; } = string.Empty; // Para saber si es imagen (image/png) o video (video/mp4)
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Propiedad de navegación (Opcional, pero recomendada)
        public Ticket? Ticket { get; set; }
    }
}