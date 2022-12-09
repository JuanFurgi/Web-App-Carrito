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
    public class CarritosController : Controller
    {
        private readonly CarritoContext _context;
        private readonly UserManager<Persona> _userManager;

        public CarritosController(CarritoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<IActionResult> Details(int? id, string? msg)
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
            var carritoContext = _context.CarritosItems.Include(c => c.Carrito).Include(c => c.Producto).Where(c => c.Carrito.ClienteId == id && c.Carrito.Activo);

            if (carritoContext == null)
            {
                return NotFound();
            }
            if (msg != null)
            {
                ViewData["Msg"] = msg;
            }

            return View(carritoContext);
        }

        public async Task<IActionResult> LimpiarCarrito(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            
            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .Include( c=> c.CarritoItems)
                .FirstOrDefaultAsync(m => m.CarritoId == id);

            if (carrito == null)
            {
                return NotFound();
            }

            for (int i = carrito.CarritoItems.Count(); i > 0; i--)
            {
                var item = carrito.CarritoItems[0];

                carrito.CarritoItems.Remove(item);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Carritos", new {id = carrito.ClienteId });
        }

        public async Task<IActionResult> DetallesProducto(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
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
