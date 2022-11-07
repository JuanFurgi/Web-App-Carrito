using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CARRITO_D.Data;
using CARRITO_D.Models;
using Microsoft.AspNetCore.Authorization;

namespace CARRITO_D.Controllers
{
    [Authorize]
    [Authorize(Roles ="Empleado")]
    public class ClientesController : Controller
    {
        private readonly CarritoContext _context;

        public ClientesController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
              return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DNI,Id,Nombre,Apellido,UserName,Email,Direccion,FechaAlta,Telefono")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DNI,Id,Nombre,Apellido,UserName,Email,Direccion,FechaAlta,Telefono")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var clienteEnDb = _context.Clientes.Find(cliente.Id);

                    if (clienteEnDb == null)
                    {
                        return NotFound();
                    }

                    clienteEnDb.Nombre = cliente.Nombre;
                    clienteEnDb.Apellido = cliente.Apellido;
                    clienteEnDb.DNI = cliente.DNI;
                    clienteEnDb.Telefono = cliente.Telefono;
                    clienteEnDb.Direccion = cliente.Direccion;
                    clienteEnDb.FechaAlta = cliente.FechaAlta;

                    if(!ActualizarMail(cliente, clienteEnDb))
                    {
                        ModelState.AddModelError("Email", "El Mail ya esta en uso");
                        return View(cliente);
                    }
                    if(!ActualizarUsuario(cliente, clienteEnDb))
                    {
                        ModelState.AddModelError("UserName", "El UserName ya esta en uso");
                        return View(cliente);
                    }

                    _context.Update(clienteEnDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        private bool ActualizarMail(Cliente cli, Cliente cliDb)
        {
            bool resultado = true;

            try
            {
                if (!cliDb.NormalizedEmail.Equals(cli.Email.ToUpper()))
                {
                    if (ExitsEmail(cli.Email))
                    {
                        resultado = false;
                    }
                    else
                    {
                        cliDb.Email = cli.Email;
                        cliDb.NormalizedEmail = cli.Email.ToUpper();
                    }
                }
            }
            catch
            {
                resultado = false;
            }

            return resultado;
        }
        private bool ActualizarUsuario(Cliente cli, Cliente cliDb)
        {
            bool resultado = true;

            try
            {
                if (!cliDb.NormalizedUserName.Equals(cli.UserName.ToUpper()))
                {
                    if (ExitsUser(cli.UserName))
                    {
                        resultado = false;
                    }
                    else
                    {
                        cliDb.UserName = cli.UserName;
                        cliDb.NormalizedUserName = cli.UserName.ToUpper();
                    }
                }
            }
            catch
            {
                resultado = false;
            }

            return resultado;
        }

        private bool ExitsEmail(string email)
        {
            return _context.Personas.Any(p => p.NormalizedEmail.Equals(email.ToUpper()));
        }

        private bool ExitsUser(string user)
        {
            return _context.Personas.Any(p => p.NormalizedUserName.Equals(user.ToUpper()));
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'CarritoContext.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
