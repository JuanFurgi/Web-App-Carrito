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
using Microsoft.AspNetCore.Identity;

namespace CARRITO_D.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        private readonly CarritoContext _context;
        private readonly UserManager<Persona> _userManager;

        public ComprasController(CarritoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Compras
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                var carritoContext = _context.Compras.Include(c => c.Cliente);
                return View(await carritoContext.ToListAsync());
            }
            else
            {
                var carritoContext = _context.Compras.Include(c => c.Cliente).Where(c => c.ClienteId == id);
                return View(await carritoContext.ToListAsync());
            }

            
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.Carrito.CarritoItems)
                .FirstOrDefaultAsync(m => m.CompraId == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Carritos/Create  
        public IActionResult Create()
         {
            int clienteId = Int32.Parse(_userManager.GetUserId(User));
            ViewData["ClienteId"] = new SelectList(_context.Clientes.Where(c => c.Id == clienteId), "Id", "Apellido");
            ViewData["CarritoId"] = new SelectList(_context.Carritos.Where(c => c.ClienteId == clienteId), "CarritoId", "CarritoId");
            return View();
         }


         // POST: Carritos/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("CompraId,Total,CarritoId,ClienteId")] Compra compra)
         {
            

             if (ModelState.IsValid)
             {
                float total = 0;
                var carritoItems = _context.Carritos.Include(c => c.CarritoItems).First(c => c.CarritoId == compra.CarritoId).CarritoItems;
                if (carritoItems != null)
                {

                    foreach (var item in carritoItems)
                    {
                        total += item.Subtotal;
                    }
                }

                Compra compraNueva = new Compra()
                {
                    Total = total,
                    CarritoId = compra.CarritoId,
                    ClienteId = compra.ClienteId
                };


                _context.Compras.Add(compraNueva);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", compra.ClienteId);
             ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id");
             return View(compra);
         }
        

        // GET: Carritos/Edit/5
        /*
         * NO VAMOS A UTILIZAR EL EDIT, NO SE DEBE EDITAR UNA COMPRA YA HECHA
         * 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }

        // POST: Carritos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarritoId,Activo,Subtotal,ClienteId")] Carrito carrito)
        {
            if (id != carrito.CarritoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoExists(carrito.CarritoId))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }
        */

        // GET: Carritos/Delete/5
        /*
         * NO VAMOS A UTILIZAR EL DELETE, NO SE DEBE ELIMINAR UNA COMPRA YA HECHA
         * 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.CarritoId == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // POST: Carritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carritos == null)
            {
                return Problem("Entity set 'CarritoContext.Carritos'  is null.");
            }
            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito != null)
            {
                _context.Carritos.Remove(carrito);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        /*
         * METODO QUE NO SE USA
        private bool CarritoExists(int id)
        {
          return _context.Carritos.Any(e => e.CarritoId == id);
        }
        */
    }
}
