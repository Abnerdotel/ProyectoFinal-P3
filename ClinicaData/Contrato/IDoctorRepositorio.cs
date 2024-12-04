using ClinicaEntidades;
using ClinicaEntidades.DTO;

namespace ClinicaData.Contrato
{
    public interface IDoctorRepositorio
    {
        Task<List<Doctor>> Lista();
        Task<string> Guardar(Doctor objeto);
        Task<string> Editar(Doctor objeto);
        Task<int> Eliminar(int Id);

        Task<string> RegistrarHorario(DoctorHorario objeto);
        Task<List<DoctorHorario>> ListaDoctorHorario();
        Task<string> EliminarHorario(int Id);
        Task<List<FechaAtencionDTO>> ListaDoctorHorarioDetalle(int Id);
        Task<List<Cita>> ListaCitasAsignadas(int Id, int IdEstadoCita);
    }
}
