using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicaData.Context;
using ClinicaData.Contrato;
using ClinicaEntidades;
using ClinicaEntidades.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClinicaData.Implementacion.Mock
{
    public class MockDoctorRepositorio : IDoctorRepositorio
    {
        private readonly DoctorContext _context;
        private readonly List<DoctorHorario> _horarios = [];
        private readonly List<Cita> _citas = [];

        public MockDoctorRepositorio(DoctorContext context)
        {
            _context = context;
        }

        public async Task<string> Guardar(Doctor objeto)
        {
            try
            {
                _context.Doctores.Add(objeto);
                await _context.SaveChangesAsync();
                return "Doctor guardado correctamente.";
            }
            catch (Exception ex)
            {
                return $"Error al guardar el doctor: {ex.Message}";
            }
        }

        public async Task<string> Editar(Doctor objeto)
        {
            try
            {
                _context.Doctores.Update(objeto);
                await _context.SaveChangesAsync();
                return "Doctor actualizado correctamente.";
            }
            catch (Exception ex)
            {
                return $"Error al editar el doctor: {ex.Message}";
            }
        }

        public async Task<int> Eliminar(int Id)
        {
            try
            {
                var doctor = await _context.Doctores.FindAsync(Id);
                if (doctor != null)
                {
                    _context.Doctores.Remove(doctor);
                    await _context.SaveChangesAsync();
                    return 1;
                }
                return 0; 
            }
            catch
            {
                return 0; 
            }
        }

        public async Task<List<Doctor>> Lista()
        {
            return await _context.Doctores.ToListAsync();
        }

        public Task<string> RegistrarHorario(DoctorHorario objeto)
        {
            _horarios.Add(objeto);
            return Task.FromResult("Horario registrado correctamente.");
        }

        public Task<List<DoctorHorario>> ListaDoctorHorario()
        {
            return Task.FromResult(_horarios);
        }

        public Task<string> EliminarHorario(int Id)
        {
            var horario = _horarios.FirstOrDefault(h => h.IdDoctorHorario == Id);
            if (horario != null)
            {
                _horarios.Remove(horario);
                return Task.FromResult("Horario eliminado correctamente.");
            }
            return Task.FromResult("Horario no encontrado.");
        }

        public Task<List<FechaAtencionDTO>> ListaDoctorHorarioDetalle(int Id)
        {
            var detalles = _horarios
                .Where(h => h.Doctor.IdDoctor == Id)
                .Select(h => new FechaAtencionDTO
                {
                    Fecha = h.DoctorHorarioDetalle.Fecha,
                    HorarioDTO = new List<HorarioDTO>
                    {
                        new HorarioDTO
                        {
                            IdDoctorHorarioDetalle = h.DoctorHorarioDetalle.IdDoctorHorarioDetalle,
                            Turno = h.DoctorHorarioDetalle.Turno,
                            TurnoHora = h.DoctorHorarioDetalle.TurnoHora
                        }
                    }
                }).ToList();

            return Task.FromResult(detalles);
        }

        public Task<List<Cita>> ListaCitasAsignadas(int Id, int IdEstadoCita)
        {
            var citasAsignadas = _citas
                .Where(c => c.Doctor.IdDoctor == Id && c.EstadoCita.IdEstadoCita == IdEstadoCita)
                .ToList();

            return Task.FromResult(citasAsignadas);
        }
    }
}