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
    public class SucursalesController : Controller
    {
        private readonly CarritoContext _context;

        public SucursalesController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Sucursales
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Sucursales.ToListAsync());
        }

        // GET: Sucursales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sucursales == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales
                .FirstOrDefaultAsync(m => m.SucursalId == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        private bool NombreUnico(string nombreSucursal)
        {
            return (_context.Sucursales.FirstOrDefault(c => c.Nombre == nombreSucursal) == null);
        }

        // GET: Sucursales/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sucursales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("SucursalId,Nombre,Direccion,Telefono,Email")] Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                if(NombreUnico(sucursal.Nombre))
                {
                    _context.Add(sucursal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Nombre", "Ya hay una sucursal con ese Nombre,\nIngrese otro");
                }   
            }
            return View(sucursal);
        }

        // GET: Sucursales/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sucursales == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null)
            {
                return NotFound();
            }
            return View(sucursal);
        }

        // POST: Sucursales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int id, [Bind("SucursalId,Nombre,Direccion,Telefono,Email")] Sucursal sucursal)
        {
            if (id != sucursal.SucursalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (NombreUnico(sucursal.Nombre))
                {
                    try
                    {
                        _context.Update(sucursal);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SucursalExists(sucursal.SucursalId))
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
                else
                {
                    ModelState.AddModelError("Nombre", "Ya hay una sucursal con ese Nombre,\nIngrese otro");
                }
                
            }
            return View(sucursal);
        }

        // GET: Sucursales/Delete/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sucursales == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales
                .FirstOrDefaultAsync(m => m.SucursalId == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // POST: Sucursales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sucursales == null)
            {
                return Problem("Entity set 'CarritoContext.Sucursales'  is null.");
            }
            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal != null)
            {
                _context.Sucursales.Remove(sucursal);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SucursalExists(int id)
        {
          return _context.Sucursales.Any(e => e.SucursalId == id);
        }


        
        // GET: Sucursales/Create
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> SumarProducto(int? id)
        {
            if (id == null || _context.Sucursales == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return RedirectToAction("Create", "StocksItems", new {id = id });
        }

        /*
        // POST: Sucursales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> SumarProducto(int id, [Bind("Id,CategoriaId,Activo,Nombre,Descripcion,PrecioVigente")] Producto producto)
        {
            

            if (id != sucursal.SucursalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
              
                StockItem stockItem = new StockItem()
                {
                    Cantidad = 1,
                    ProductoId = producto.Id,
                    SucursalId = sucursal.SucursalId
                };
                sucursal.StockItems.Add(stockItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(producto);
        }
        */
        
    }
}
