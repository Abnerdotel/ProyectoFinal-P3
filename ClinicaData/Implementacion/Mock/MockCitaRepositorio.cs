using ClinicaData.Contrato;
using ClinicaData.Context;
using ClinicaEntidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaData.Implementacion.Mock
{
    public class MockCitaRepositorio : ICitaRepositorio
    {
        private readonly CitasContext _context;

        public MockCitaRepositorio(CitasContext context)
        {
            _context = context;
            CargarDatosIniciales();
        }

        public async Task<string> CambiarEstado(int IdCita, int IdEstado, string Indicaciones)
        {
            var cita = await _context.Citas.FindAsync(IdCita);
            if (cita == null)
                return "Cita no encontrada";

            cita.EstadoCita = new EstadoCita { IdEstadoCita = IdEstado };
            //TODO
            //No tengo idea de como implementas el estado, asi que lo dejo a tu parecer
            cita.Indicaciones = Indicaciones;

            _context.Citas.Update(cita);
            await _context.SaveChangesAsync();

            return "Estado actualizado correctamente";
        }

        public async Task<string> Cancelar(int Id)
        {
            var cita = await _context.Citas.FindAsync(Id);
            if (cita == null)
                return "Cita no encontrada";

            cita.EstadoCita = new EstadoCita { IdEstadoCita = 3 }; 
            //TODO
            //Lo mismo,omo ni idea de como estas implementando el estado de citas, puse ese ID
            _context.Citas.Update(cita);
            await _context.SaveChangesAsync();

            return "Cita cancelada correctamente";
        }

        public async Task<string> Guardar(Cita objeto)
        {
            _context.Citas.Add(objeto);
            await _context.SaveChangesAsync();
            return "Cita guardada correctamente";
        }

        public async Task<List<Cita>> ListaCitasPendiente(int IdUsuario)
        {
            return await Task.FromResult(_context.Citas
                .Where(c => c.Usuario.IdUsuario == IdUsuario && c.EstadoCita.IdEstadoCita == 1)
                //TODO
                //Lo mismo aqui, implementa con consideres
                .ToList());
        }

        public async Task<List<Cita>> ListaHistorialCitas(int IdUsuario)
        {
            return await Task.FromResult(_context.Citas
                .Where(c => c.Usuario.IdUsuario == IdUsuario && c.EstadoCita.IdEstadoCita != 1)
                .ToList());
        }

        private void CargarDatosIniciales()
        {
            if (!_context.Citas.Any())
            {
                _context.Citas.AddRange(new List<Cita>
                {
                    new Cita
                    {
                        IdCita = 1,
                        Usuario = new Usuario { IdUsuario = 1 },
                        DoctorHorarioDetalle = new DoctorHorarioDetalle { IdDoctorHorarioDetalle = 1 },
                        EstadoCita = new EstadoCita { IdEstadoCita = 1 }, // TODO
                        FechaCita = "2024-12-10",
                        HoraCita = "10:00 AM",
                        Indicaciones = "Indicaciones iniciales"
                    },
                    new Cita
                    {
                        IdCita = 2,
                        Usuario = new Usuario { IdUsuario = 1 },
                        DoctorHorarioDetalle = new DoctorHorarioDetalle { IdDoctorHorarioDetalle = 2 },
                        EstadoCita = new EstadoCita { IdEstadoCita = 2 }, // TODO
                        FechaCita = "2024-11-15",
                        HoraCita = "02:00 PM",
                        Indicaciones = "Consulta completada"
                    }
                });

                _context.SaveChanges();
            }
        }
    }
}
