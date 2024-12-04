
namespace ClinicaEntidades.DTO
{
    public class FechaAtencionDTO
    {
        public string Fecha { get; set; } = null!;
        public List<HorarioDTO> HorarioDTO { get; set; } = null!;
    }
}
