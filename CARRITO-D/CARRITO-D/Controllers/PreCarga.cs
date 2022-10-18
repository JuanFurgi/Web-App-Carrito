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

        private readonly List<string> roles = new List<string>() { "Admin", "Cliente", "Empleado", "Usuario" };

        public PreCarga(UserManager<Persona> userManager, RoleManager<Rol> roleManager, CarritoContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
        }


        public IActionResult Seed()
        {
            CrearRoles().Wait();
            CrearAdmin().Wait();
            CrearEmpleados().Wait();
            CrearClientes().Wait();
            CrearProductos().Wait();

            return RedirectToAction("Index", "Home", new { mensaje="Proceso de Seed Finalizado"});
        }

        private async Task CrearProductos()
        {
            
        }

        private async Task CrearClientes()
        {
            
        }

        private async Task CrearEmpleados()
        {
            
        }

        private async Task CrearAdmin()
        {

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
