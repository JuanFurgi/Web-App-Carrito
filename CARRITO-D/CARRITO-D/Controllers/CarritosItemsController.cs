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
    public class CarritosItemsController : Controller
    {
        private readonly CarritoContext _context;
        private readonly UserManager<Persona> _userManager;

        public CarritosItemsController(CarritoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public IActionResult Create(int? id)
        {
            
            ViewData["CarritoId"] = new SelectList(_context.Carritos.Where(c => c.ClienteId == int.Parse(_userManager.GetUserId(User))), "CarritoId", "CarritoId");
            if(id == null)
            {
                ViewData["ProductoNombre"] = new SelectList(_context.Productos, "Id", "Nombre");
            }
            else
            {
                ViewData["ProductoNombre"] = new SelectList(_context.Productos.Where(c => c.Id == id), "Id", "Nombre");
            }
            return View();
        }

        // POST: CarritosItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ValorUnitario,Cantidad,CarritoId,ProductoId")] CarritoItem carritoItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.CarritosItems.Add(carritoItem);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Carritos", new {id = _userManager.GetUserId(User) });
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, $"Error: Producto ya está agregado al carrito");
                }
                
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos.Where(c => c.ClienteId == int.Parse(_userManager.GetUserId(User))), "CarritoId", "CarritoId");
            ViewData["ProductoNombre"] = new SelectList(_context.Productos.Where(c => c.Id == carritoItem.ProductoId), "Id", "Nombre");
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
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var carritoItem = await _context.CarritosItems.FirstAsync(c => c.Id == id);
            if (carritoItem != null)
            {
                _context.CarritosItems.Remove(carritoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Details",  "Carritos", new {id = _userManager.GetUserId(User)});
        }

        private bool CarritoItemExists(int id)
        {
          return _context.CarritosItems.Any(e => e.CarritoId == id);
        }


        public IActionResult Sumar(int? id)
        {
            if (id == null || _context.CarritosItems == null)
            {
                return NotFound();
            }

            var carritoItem = _context.CarritosItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefault(m => m.ProductoId == id);
            if (carritoItem == null)
            {
                return NotFound();
            }

            carritoItem.Cantidad++;
            _context.SaveChanges();
            return RedirectToAction("Details", "Carritos", new {id = _userManager.GetUserId(User)});
        }

        public async Task<IActionResult> Restar(int? id)
        {
            if (id == null || _context.CarritosItems == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritosItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (carritoItem == null)
            {
                return NotFound();
            }

            if(carritoItem.Cantidad <= 1)
            {
                await DeleteConfirmed(carritoItem.Id);
                //ModelState.AddModelError(string.Empty, "No se pueden restar mas productos");
                //ModelState.AddModelError(string.Empty, "Se elimino el producto del carrito");
                //return RedirectToAction(nameof(Index));
            }

            carritoItem.Cantidad--;
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Carritos", new { id = _userManager.GetUserId(User) });
        }

        private void eliminarCarritoItem(CarritoItem carritoItem)
        {
            //_context.Carritos.Include(c => c.CarritoItems).First(c => c.CarritoItems.First(c => c.Id == id) != null);
            Carrito carrito = _context.Carritos.Include(c => c.CarritoItems).First(c => c.CarritoId == carritoItem.CarritoId);
            carrito.CarritoItems.Remove(carritoItem);
            _context.SaveChanges();
        }
    }
}
