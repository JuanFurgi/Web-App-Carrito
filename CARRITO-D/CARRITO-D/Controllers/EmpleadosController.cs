using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CARRITO_D.Data;
using CARRITO_D.Models;
using Microsoft.AspNetCore.Identity;
using CARRITO_D.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CARRITO_D.Controllers
{
    [Authorize]
    [Authorize(Roles = "Empleado")]
    public class EmpleadosController : Controller
    {
        private readonly CarritoContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;

        public EmpleadosController(CarritoContext context, UserManager<Persona> usermanager, SignInManager<Persona>signInManager)
        {
            _context = context;
            this._userManager = usermanager;
            this._signInManager = signInManager;
        }

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
              return View(await _context.Empleados.ToListAsync());
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Legajo,Id,Nombre,Apellido,UserName,Email,Direccion,FechaAlta,Telefono")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                var resultadoCreate = await _userManager.CreateAsync(empleado, Configs.Password);

                if (resultadoCreate.Succeeded)
                {
                    var resultadoAddRole = await _userManager.AddToRoleAsync(empleado, Configs.EmpleadoRolName);

                    if (resultadoAddRole.Succeeded)
                    {
                        await _signInManager.SignInAsync(empleado, false);
                        return RedirectToAction("Index", "Empleados", new { id = empleado.Id });
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, $"No se pudo agregar el rol de {Configs.EmpleadoRolName}");
                    }

                }

                foreach (var error in resultadoCreate.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(empleado);
        }


        /*
         * NO SE PUEDE EDITAR UN EMPLEADO
        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Legajo,Id,Nombre,Apellido,UserName,Email,Direccion,FechaAlta,Telefono")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var empleadoEnDb = _context.Empleados.Find(empleado.Id);

                    if (empleadoEnDb == null)
                    {
                        return NotFound();
                    }

                    empleadoEnDb.Legajo = empleado.Legajo;
                    empleadoEnDb.Nombre = empleado.Nombre;
                    empleadoEnDb.Apellido = empleado.Apellido;
                    empleadoEnDb.UserName = empleado.UserName;
                    empleadoEnDb.Email = empleado.Email;
                    empleadoEnDb.Direccion = empleado.Direccion;
                    empleadoEnDb.FechaAlta = empleado.FechaAlta;
                    empleadoEnDb.Telefono = empleado.Telefono;

                    _context.Empleados.Update(empleadoEnDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(empleado);
        }
        */

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleados == null)
            {
                return Problem("Entity set 'CarritoContext.Empleados'  is null.");
            }
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
          return _context.Empleados.Any(e => e.Id == id);
        }

        public async Task<IActionResult> VerHistorial()
        {
            if (_context.Compras == null)
            {
                return Problem("No hay compras");
            }

            return RedirectToAction("Index", "Compras");
        }
    }
}
