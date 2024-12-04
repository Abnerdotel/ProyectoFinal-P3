using ClinicaEntidades;


namespace ClinicaData.Contrato
{
    public interface IRolUsuarioRepositorio
    {
        Task<List<RolUsuario>> Lista();
    }
}
