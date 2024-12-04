using ClinicaEntidades;
using Microsoft.EntityFrameworkCore;

namespace ClinicaData.Context
{
    public class DoctorContext : DbContext
    {
        public DoctorContext(DbContextOptions options) : base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("ClinicaCitasDB");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Doctor> Doctores { get; set;}
    }
}