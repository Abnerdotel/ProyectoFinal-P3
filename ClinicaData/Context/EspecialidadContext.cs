using ClinicaEntidades;
using Microsoft.EntityFrameworkCore;

namespace ClinicaData.Context
{
    public class EspecialidadContext : DbContext
    {
        public EspecialidadContext(DbContextOptions options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("ClinicaCitasDB");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Especialidad> Especialidades { get; set; }
    }
}