namespace ITSOLUTION.Application.DTOs
{
    public class DashboardDto
    {
        public KpiDto Kpis { get; set; } = new();
        public List<AlertDto> RecentAlerts { get; set; } = new();
    }

    public class KpiDto
    {
        public string SystemUptime { get; set; } = string.Empty;
        public int ActiveTickets { get; set; }
        public int SecurityAlerts { get; set; }
        public int PendingUpdates { get; set; }
    }

    public class AlertDto
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Critical", "Warning", "Info"
        public string Message { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
    }
}