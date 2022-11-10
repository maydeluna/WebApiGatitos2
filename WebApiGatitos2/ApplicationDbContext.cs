using Microsoft.EntityFrameworkCore;
using WebApiGatos.Entidades;

namespace WebApiGatos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Dueño> Dueños { get; set; }
        public DbSet<Gatos> Gatos { get; set; }
    }
}
