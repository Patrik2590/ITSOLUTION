namespace ITSOLUTION.Application.DTOs
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string CorreoCorporativo { get; set; } = string.Empty;
        public bool EstaActivo { get; set; }
    }

    public class CreateEmpleadoDto
    {
        public string Cedula { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string CorreoCorporativo { get; set; } = string.Empty;
    }
}