using ClinicaEntidades;
using Microsoft.EntityFrameworkCore;

namespace ClinicaData.Context
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("ClinicaCitasDB");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RolUsuario> RolesUsuario { get; set; }
    }
}
