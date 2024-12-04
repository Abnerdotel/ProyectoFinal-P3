using ClinicaData.Contrato;
using ClinicaData.Context;
using ClinicaEntidades;
using Microsoft.EntityFrameworkCore;

namespace ClinicaData.Implementacion.Mock
{
    public class MockRolUsuarioRepositorio : IRolUsuarioRepositorio
    {
        private readonly RolUsuarioContext _context;

        public MockRolUsuarioRepositorio(RolUsuarioContext context)
        {
            _context = context;
        }

        public async Task<List<RolUsuario>> Lista()
        {
            return await _context.RolesUsuario.ToListAsync();
        }
    }
}
