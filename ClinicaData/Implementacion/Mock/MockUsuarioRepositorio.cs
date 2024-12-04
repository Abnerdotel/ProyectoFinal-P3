using ClinicaData.Context;
using ClinicaData.Contrato;
using ClinicaEntidades;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaData.Implementacion
{
    public class MockUsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly UsuarioContext _context;

        public MockUsuarioRepositorio(UsuarioContext context)
        {
            _context = context;
        }

        public async Task<string> Editar(Usuario objeto)
        {
            string respuesta;

            try
            {
                var usuarioExistente = await _context.Usuarios
                    .Include(u => u.RolUsuario) 
                    .FirstOrDefaultAsync(u => u.IdUsuario == objeto.IdUsuario);

                if (usuarioExistente != null)
                {
                    usuarioExistente.NumeroDocumentoIdentidad = objeto.NumeroDocumentoIdentidad;
                    usuarioExistente.Nombre = objeto.Nombre;
                    usuarioExistente.Apellido = objeto.Apellido;
                    usuarioExistente.Correo = objeto.Correo;
                    usuarioExistente.Clave = objeto.Clave;
                    usuarioExistente.RolUsuario = objeto.RolUsuario; 

                    await _context.SaveChangesAsync();
                    respuesta = "Usuario editado con éxito";
                }
                else
                {
                    respuesta = "Usuario no encontrado";
                }
            }
            catch
            {
                respuesta = "Error al editar usuario";
            }
            return respuesta;
        }

        public async Task<int> Eliminar(int Id)
        {
            int respuesta = 1;
            try
            {
                var usuarioExistente = await _context.Usuarios.FindAsync(Id);
                if (usuarioExistente != null)
                {
                    _context.Usuarios.Remove(usuarioExistente);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    respuesta = 0; 
                }
            }
            catch
            {
                respuesta = 0; 
            }
            return respuesta;
        }

        public async Task<string> Guardar(Usuario objeto)
        {
            string respuesta;
            try
            {
                await _context.Usuarios.AddAsync(objeto);
                await _context.SaveChangesAsync();
                respuesta = "Usuario guardado con éxito";
            }
            catch
            {
                respuesta = "Error al guardar usuario";
            }

            return respuesta;
        }
        public async Task<List<Usuario>> Lista(int IdRolUsuario = 0)
        {
            IQueryable<Usuario> query = _context.Usuarios.Include(u => u.RolUsuario); 

            if (IdRolUsuario > 0)
            {
            query = query.Where(u => u.RolUsuario.IdRolUsuario == IdRolUsuario);
            }

            return await query.ToListAsync();
        }

        public async Task<Usuario> Login(string DocumentoIdentidad, string Clave)
        {
            return await _context.Usuarios
            .Include(u => u.RolUsuario) 
            .FirstOrDefaultAsync(u => u.NumeroDocumentoIdentidad == DocumentoIdentidad && u.Clave == Clave);
        }
    }
}
