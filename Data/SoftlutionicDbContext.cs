using Microsoft.EntityFrameworkCore;
using SoftlutionicAPI.Models;

namespace SoftlutionicAPI.Data
{
    public class SoftlutionicDbContext : DbContext
    {
        public SoftlutionicDbContext(DbContextOptions<SoftlutionicDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contacto> Contactos { get; set; }
    }
}
