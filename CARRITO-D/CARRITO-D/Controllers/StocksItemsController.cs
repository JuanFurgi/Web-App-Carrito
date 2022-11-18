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
    [Authorize(Roles = "Empleado")]
    public class StocksItemsController : Controller
    {
        private readonly CarritoContext _context;

        public StocksItemsController(CarritoContext context)
        {
            _context = context;
        }

        // GET: StocksItems
        public async Task<IActionResult> Index()
        {
            var carritoContext = _context.StocksItems.Include(s => s.Producto).Include(s => s.Sucursal);
            return View(await carritoContext.ToListAsync());
        }

        // GET: StocksItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StocksItems == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StocksItems
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.SucursalId == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // GET: StocksItems/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre");
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "SucursalId", "Nombre");
            return View();
        }

        // POST: StocksItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cantidad,SucursalId,ProductoId")] StockItem stockItem)
        {
            //ERROR EN SAVE CHANGES PORQUE HAY MIGRACIONES CON LAS QUE NO SE AVANZARON POR ERRORES
            if (ModelState.IsValid)
            {
                _context.Add(stockItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "SucursalId", "Nombre", stockItem.SucursalId);
            return View(stockItem);
        }

        // GET: StocksItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StocksItems == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StocksItems.FindAsync(id);
            if (stockItem == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Id", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "SucursalId", "SucursalId", stockItem.SucursalId);
            return View(stockItem);
        }

        // POST: StocksItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cantidad,SucursalId,ProductoId")] StockItem stockItem)
        {
            if (id != stockItem.SucursalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockItemExists(stockItem.SucursalId))
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
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Id", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "SucursalId", "SucursalId", stockItem.SucursalId);
            return View(stockItem);
        }

        // GET: StocksItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StocksItems == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StocksItems
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.SucursalId == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // POST: StocksItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StocksItems == null)
            {
                return Problem("Entity set 'CarritoContext.StocksItems'  is null.");
            }
            var stockItem = await _context.StocksItems.FindAsync(id);
            if (stockItem != null)
            {
                _context.StocksItems.Remove(stockItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockItemExists(int id)
        {
          return _context.StocksItems.Any(e => e.SucursalId == id);
        }
    }
}
