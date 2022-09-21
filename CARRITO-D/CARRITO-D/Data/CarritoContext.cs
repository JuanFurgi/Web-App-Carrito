using CARRITO_D.Models;
using Microsoft.EntityFrameworkCore;

namespace CARRITO_D.Data
{
    public class CarritoContext : DbContext
    {

        public CarritoContext(DbContextOptions options) : base(options)
        {

        }
        
        public DbSet<Persona> personas { get; set; } 

    }
}
