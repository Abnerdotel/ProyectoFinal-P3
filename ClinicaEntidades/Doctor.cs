
namespace ClinicaEntidades
{
    public class Doctor
    {
        public int IdDoctor { get; set; }
        public string NumeroDocumentoIdentidad { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Genero { get; set; } = null!;
        public Especialidad Especialidad { get; set; } = null!;
        public string FechaCreacion { get; set; } = null!;

    }
}
