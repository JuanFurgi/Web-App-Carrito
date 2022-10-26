using CARRITO_D.Data;
using CARRITO_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CARRITO_D.Controllers
{
    public class PreCarga : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly CarritoContext _context;

        private readonly List<string> roles = new List<string>() {"Cliente", "Empleado", "Usuario" };

        public PreCarga(UserManager<Persona> userManager, RoleManager<Rol> roleManager, CarritoContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
        }


        public IActionResult Seed()
        {
            CrearRoles().Wait();
            CrearEmpleados().Wait();
            CrearClientes().Wait();
            CrearCategoria().Wait();
            CrearProductos();

            return RedirectToAction("Index", "Home", new { mensaje="Proceso de Seed Finalizado"});
        }

        private async Task CrearCategoria()
        {
            Categoria categoria = new Categoria()
            {
                Nombre = "Bebidas",
            };
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

        }

        private void CrearProductos()
        {
            if (_context.Categorias.Any())
            {
                Producto producto = new Producto()
                {
                    Nombre = "Coca",
                    CategoriaId = _context.Categorias.First().CategoriaId,
                    Activo = true,
                    PrecioVigente = 120,
                    Descripcion = "Bebida marca Coca-Company"
                };
                _context.Productos.Add(producto);
                _context.SaveChanges();
            }

            

            
        }

        private async Task CrearClientes()
        {
            await _userManager.CreateAsync(new Cliente());
        }

        private async Task CrearEmpleados()
        {
            await _userManager.CreateAsync(new Empleado());
        }


        private async Task CrearRoles()
        {
            foreach(var rolName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rolName))
                {
                    await _roleManager.CreateAsync(new Rol(rolName));
                }
            }
        }
    }
}
