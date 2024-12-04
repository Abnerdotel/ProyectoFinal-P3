using ClinicaEntidades;
using Microsoft.EntityFrameworkCore;

namespace ClinicaData.Context
{
    public class RolUsuarioContext : DbContext
    {
        public RolUsuarioContext(DbContextOptions options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("ClinicaCitasDB");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<RolUsuario> RolesUsuario { get; set; }
    }
}
