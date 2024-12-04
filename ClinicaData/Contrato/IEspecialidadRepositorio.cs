using ClinicaEntidades;

namespace ClinicaData.Contrato
{
    public interface IEspecialidadRepositorio
    {
        Task<List<Especialidad>> Lista();
        Task<string> Guardar(Especialidad objeto);
        Task<string> Editar(Especialidad objeto);
        Task<int> Eliminar(int Id);
    }
}
