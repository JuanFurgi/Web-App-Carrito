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
    public class CarritosController : Controller
    {
        private readonly CarritoContext _context;

        public CarritosController(CarritoContext context)
        {
            _context = context;
        }

        /*
         * ELIMINAMOS EL INDEX PORQUE NO LO VAMOS A USAR
         * 
        // GET: Carritos
        public async Task<IActionResult> Index()
        {
            var carritoContext = _context.Carritos.Include(c => c.Cliente);
            return View(await carritoContext.ToListAsync());
        }
        */

        // GET: Carritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            /*
            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .Include( c=> c.CarritoItems)
                .FirstOrDefaultAsync(m => m.CarritoId == id);
            */
            var carritoContext = _context.CarritosItems.Include(c => c.Carrito).Include(c => c.Producto).Where(c => c.Carrito.ClienteId == id);

            if (carritoContext == null)
            {
                return NotFound();
            }

            return View(carritoContext);
        }

        // GET: Carritos/Create
        /* 
         * ELIMINAMOS CREATE PORQUE NO VA A SER USADO
         * 
         * public IActionResult Create()
         {
             ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
             return View();
         }


         // POST: Carritos/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("CarritoId,Activo,Subtotal,ClienteId")] Carrito carrito)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(carrito);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
             return View(carrito);
         }
        */

        // GET: Carritos/Edit/5
        /*
         * NO VAMOS A UTILIZAR EL EDIT, LO VAMOS A IMPLEMENTAR NOSOTROS
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
         * NO VAMOS A UTILIZAR EL DELETE, LO VAMOS A IMPLEMENTAR NOSOTROS
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
