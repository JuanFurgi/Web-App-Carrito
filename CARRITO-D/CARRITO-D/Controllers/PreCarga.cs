﻿using CARRITO_D.Data;
using CARRITO_D.Helpers;
using CARRITO_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CARRITO_D.Controllers
{
    public class PreCarga : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly CarritoContext _context;
        private readonly SignInManager<Persona> _signInManager;
        private readonly List<string> roles = new List<string>() {"Cliente", "Empleado"};

        public PreCarga(UserManager<Persona> userManager, RoleManager<Rol> roleManager, CarritoContext context, SignInManager<Persona> signInManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
            this._signInManager = signInManager;
        }


        public IActionResult Seed()
        {
            CrearRoles().Wait();
 Carrito            CrearCarrito().Wait();
            CrearEmpleados().Wait();
            CrearClientes().Wait();
            CrearCategoria().Wait();
            CrearProductos();

            return RedirectToAction("Index", "Home", new { mensaje="Proceso de Seed Finalizado"});
        }

        private async Task CrearCarrito()
        {
            Carrito carrito = new Carrito()
            {
                Activo = true,
                Subtotal = 0,
                CarritoItems = new List<CarritoItem>(),
                ClienteId = _context.Clientes.First().Id
            };
            _context.Carritos.Add(carrito);
            _context.SaveChanges();
        }

        private async Task CrearCategoria()
        {
            Categoria categoria1 = new Categoria()
            {
                Nombre = "Camisetas",
                Descripcion = "Es una prenda de ropa interior de abrigo por lo general de mangas cortas, cuello redondo o en forma de 'V'"// Camisetas y tmb pantalones, shorts, buzos
            };
            Categoria categoria2 = new Categoria()
            {
                Nombre = "Pantalones",
                Descripcion = "Prenda de vestir que se ajusta a la cintura y llega generalmente hasta el pie , cubriendo cada pierna separadamente"// Camisetas y tmb pantalones, shorts, buzos
            };
            Categoria categoria3 = new Categoria()
            {
                Nombre = "Shorts",
                Descripcion = "pantalones cortos de toda la vida que se reinventan año tras año para adaptarse a las nuevas tendencias que dicta la industria de la moda"// Camisetas y tmb pantalones, shorts, buzos
            };
            Categoria categoria4 = new Categoria()
            {
                Nombre = "Buzos",
                Descripcion = "Prenda deportiva que cubre el torso, generalmente con capucha"// Camisetas y tmb pantalones, shorts, buzos
            };
            _context.Categorias.Add(categoria1);
            _context.Categorias.Add(categoria2);
            _context.Categorias.Add(categoria3);
            _context.Categorias.Add(categoria4);
            _context.SaveChanges();

        }

        private Categoria encontrarCategoria(string nombreCategoria)
        {
            return _context.Categorias.First(c => c.Nombre == nombreCategoria);
        }

        private void CrearProductos()
        {
            if (_context.Categorias.Any())
            {
                Producto producto1 = new Producto()
                {
                    Nombre = "Camiseta",
                    CategoriaId = _context.Categorias.First().CategoriaId,
                    Activo = true,
                    PrecioVigente = 8000,
                    Descripcion = "Camiseta Beige con logo de Klouth en el centro, minimalista"
                };
                Producto producto2 = new Producto()
                {
                    Nombre = "Pantalon",
                    CategoriaId = encontrarCategoria("Pantalones").CategoriaId,
                    Activo = false,
                    PrecioVigente = 5000,
                    Descripcion = "Pantalon de jean marca Klouth"
                };
                Producto producto3 = new Producto()
                {
                    Nombre = "Short",
                    CategoriaId = encontrarCategoria("Shorts").CategoriaId,
                    Activo = true,
                    PrecioVigente = 2500,
                    Descripcion = "Short negro marca Klouth, con hebilla color dorado"
                };
                Producto producto4 = new Producto()
                {
                    Nombre = "Buzo",
                    CategoriaId = encontrarCategoria("Buzos").CategoriaId,
                    Activo = true,
                    PrecioVigente = 10000,
                    Descripcion = "Buzo negro con logo de Klouth, con capucha y detalles dorados",
                };
                _context.Productos.Add(producto1);
                _context.Productos.Add(producto2);
                _context.Productos.Add(producto3);
                _context.Productos.Add(producto4);
                _context.SaveChanges();
            }

            

            
        }

        private async Task CrearClientes()
        {

            Cliente clienteNuevo = new
                Cliente()
            {
                UserName = "Cliente1",
                Email = "cliente1@ort.edu.ar",
                Nombre = "Pedro",
                Apellido = "Picapiedra",
                DNI = 45233213,
                Direccion = "Vicente Lopez 789",
                Telefono = 1123456789, //esto es igual a PhoneNumber? hace el override?
                FechaAlta = DateTime.Now,
                Carritos = new List<Carrito>(),
                
               
            };

            clienteNuevo.Carritos.Add(_context.Carritos.First());

            var resultadoCreate = await _userManager.CreateAsync(clienteNuevo, Configs.Password);

            if (resultadoCreate.Succeeded)
            {
                await _userManager.AddToRoleAsync(clienteNuevo, Configs.ClienteRolName);

            }
        }

        private async Task CrearEmpleados()
        {
            Empleado empleadoNuevo = new
                Empleado()
            {
                UserName = "Empleado1",
                Email = "empleado1@ort.edu.ar",
                Nombre = "Pablo",
                Apellido = "Picapiedra",
                Direccion = "CABA, Belgrano 2033",
                Telefono = 1109876543,
                Legajo = 109234,
                FechaAlta = DateTime.Now,

            };

            var resultadoCreate = await _userManager.CreateAsync(empleadoNuevo, Configs.Password);

            if (resultadoCreate.Succeeded)
            {
                await _userManager.AddToRoleAsync(empleadoNuevo, Configs.EmpleadoRolName);
            }
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
