using CARRITO_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CARRITO_D.Data
{
    public class CarritoContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {

        public CarritoContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarritoItem>().HasKey(ci => new { ci.CarritoId, ci.ProductoId});

            modelBuilder.Entity<CarritoItem>()
                .HasOne(ci => ci.Carrito)
                .WithMany(c => c.CarritoItems)
                .HasForeignKey(ci => ci.CarritoId);

            modelBuilder.Entity<CarritoItem>()
                .HasOne(ci => ci.Producto)
                .WithMany(i => i.CarritosItem)
                .HasForeignKey(ci => ci.ProductoId);


            modelBuilder.Entity<StockItem>().HasKey(si => new { si.SucursalId, si.ProductoId });

            modelBuilder.Entity<StockItem>()
                .HasOne(si => si.Sucursal)
                .WithMany(s => s.StockItems)
                .HasForeignKey(si => si.SucursalId);

            modelBuilder.Entity<StockItem>()
                .HasOne(si => si.Producto)
                .WithMany(i => i.StocksItem)
                .HasForeignKey(si => si.ProductoId);


            #region Establecer Nombres para IdentityStores


            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");

            #endregion
        }

        public DbSet<Persona> Personas { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Carrito> Carritos { get; set; }

        public DbSet<CarritoItem> CarritosItems { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Compra> Compras { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<StockItem> StocksItems { get; set; }

        public DbSet<Sucursal> Sucursales { get; set; }

        public DbSet<Rol> Roles { get; set; }


    }
}
