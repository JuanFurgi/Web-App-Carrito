using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CARRITO_D.Data;
using CARRITO_D.Models;

namespace CARRITO_D.Controllers
{
    public class CarritosItemsController : Controller
    {
        private readonly CarritoContext _context;

        public CarritosItemsController(CarritoContext context)
        {
            _context = context;
        }

        // GET: CarritosItems
        public async Task<IActionResult> Index()
        {
            var carritoContext = _context.CarritosItems.Include(c => c.Carrito).Include(c => c.Producto);
            return View(await carritoContext.ToListAsync());
        }

        // GET: CarritosItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CarritosItems == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritosItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(m => m.CarritoId == id);
            if (carritoItem == null)
            {
                return NotFound();
            }

            return View(carritoItem);
        }

        // GET: CarritosItems/Create
        public IActionResult Create(int? productoId)
        {
            if(productoId == null)
            {
                return NotFound();
            }

            var username = User.Identity.Name;
            var usuarioEnDb = _context.Clientes.Include(c => c.Carritos).FirstOrDefault(c => c.NormalizedUserName == username.ToUpper());
            var carritoDelUIsuario = usuarioEnDb.Carritos.FirstOrDefault(c => c.Activo);

            var productoEnDb = _context.Productos.Find(productoId);

            CarritoItem ci = new CarritoItem();

            ci.CarritoId = carritoDelUIsuario.CarritoId;
            ci.ProductoId = productoId.Value;
            ci.ValorUnitario = productoEnDb.PrecioVigente;
            ci.Cantidad = 1;
            
            
            return View(ci);
        }

        // POST: CarritosItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Subtotal,ValorUnitario,Cantidad,CarritoId,ProductoId")] CarritoItem carritoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carritoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId", carritoItem.CarritoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Id", carritoItem.ProductoId);
            return View(carritoItem);
        }

        // GET: CarritosItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CarritosItems == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritosItems.FindAsync(id);
            if (carritoItem == null)
            {
                return NotFound();
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId", carritoItem.CarritoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Id", carritoItem.ProductoId);
            return View(carritoItem);
        }

        // POST: CarritosItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Subtotal,ValorUnitario,Cantidad,CarritoId,ProductoId")] CarritoItem carritoItem)
        {
            if (id != carritoItem.CarritoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carritoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoItemExists(carritoItem.CarritoId))
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
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId", carritoItem.CarritoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Id", carritoItem.ProductoId);
            return View(carritoItem);
        }

        // GET: CarritosItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CarritosItems == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritosItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(m => m.CarritoId == id);
            if (carritoItem == null)
            {
                return NotFound();
            }

            return View(carritoItem);
        }

        // POST: CarritosItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CarritosItems == null)
            {
                return Problem("Entity set 'CarritoContext.CarritosItems'  is null.");
            }
            var carritoItem = await _context.CarritosItems.FindAsync(id);
            if (carritoItem != null)
            {
                _context.CarritosItems.Remove(carritoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarritoItemExists(int id)
        {
          return _context.CarritosItems.Any(e => e.CarritoId == id);
        }
    }
}
