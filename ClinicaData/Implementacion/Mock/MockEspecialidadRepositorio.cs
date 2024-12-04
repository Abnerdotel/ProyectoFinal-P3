using ClinicaData.Contrato;
using ClinicaData.Context;
using ClinicaEntidades;
using Microsoft.EntityFrameworkCore;

namespace ClinicaData.Implementacion.Mock
{
    public class MockEspecialidadRepositorio : IEspecialidadRepositorio
    {
        private readonly EspecialidadContext _context;

        public MockEspecialidadRepositorio(EspecialidadContext context)
        {
            _context = context;
        }

        public async Task<List<Especialidad>> Lista()
        {
            return await _context.Especialidades.ToListAsync();
        }

        public async Task<string> Guardar(Especialidad objeto)
        {
            try
            {
                await _context.Especialidades.AddAsync(objeto);
                await _context.SaveChangesAsync();
                return "Especialidad guardada exitosamente.";
            }
            catch (Exception ex)
            {
                return $"Error al guardar la especialidad: {ex.Message}";
            }
        }

        public async Task<string> Editar(Especialidad objeto)
        {
            try
            {
                var especialidadExistente = await _context.Especialidades
                    .FirstOrDefaultAsync(e => e.IdEspecialidad == objeto.IdEspecialidad);

                if (especialidadExistente == null)
                    return "Especialidad no encontrada.";

                especialidadExistente.Nombre = objeto.Nombre;
                especialidadExistente.FechaCreacion = objeto.FechaCreacion;

                _context.Especialidades.Update(especialidadExistente);
                await _context.SaveChangesAsync();

                return "Especialidad actualizada exitosamente.";
            }
            catch (Exception ex)
            {
                return $"Error al editar la especialidad: {ex.Message}";
            }
        }

        public async Task<int> Eliminar(int Id)
        {
            try
            {
                var especialidad = await _context.Especialidades
                    .FirstOrDefaultAsync(e => e.IdEspecialidad == Id);

                if (especialidad == null)
                    return 0; 

                _context.Especialidades.Remove(especialidad);
                await _context.SaveChangesAsync();

                return 1; 
            }
            catch
            {
                return 0; 
            }
        }
    }
}
