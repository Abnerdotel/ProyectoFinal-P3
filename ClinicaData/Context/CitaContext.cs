using Microsoft.EntityFrameworkCore;
using ClinicaEntidades;

namespace ClinicaData.Context
{
    public class CitasContext : DbContext
    {
        public CitasContext(DbContextOptions<CitasContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("ClinicaCitasDB");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Cita> Citas { get; set; }
    }
}
