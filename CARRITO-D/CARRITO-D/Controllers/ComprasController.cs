using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CARRITO_D.ViewModels;
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
                var carritoContext = _context.Compras.Include(c => c.Cliente).Include(c => c.Carrito).OrderBy(c => c.Fecha);
                return View(await carritoContext.ToListAsync());
            }
            else
            {
                var carritoContext = _context.Compras.Include(c => c.Cliente).Include(c => c.Carrito).Where(c => c.ClienteId == id).OrderBy(c => c.Fecha);
                return View(carritoContext);
            }

            
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            CompraItems mymodel = new CompraItems();
            mymodel.Compra = await _context.Compras
                .Include(c => c.Cliente)
                .Include(c => c.Carrito.CarritoItems)
                .FirstOrDefaultAsync(m => m.CompraId == id);

            

            if (mymodel.Compra == null)
            {
                return NotFound();
            }

            mymodel.Items = _context.CarritosItems.Include(c => c.Producto).Where(c => c.CarritoId == _context.Compras.Find(id).CarritoId);

            return View(mymodel);
        }

        // GET: Carritos/Create  
        public IActionResult Create(int? id)
         {
            ViewData["TotalValue"] = 0;
            ViewData["ClienteId"] = id;
            ViewData["CarritoId"] = _context.Carritos.First(c => c.ClienteId == id && c.Activo).CarritoId;
            ViewData["Sucursales"] = new SelectList(_context.Sucursales,"SucursalId", "Nombre");
            return View();
         }


         // POST: Carritos/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("SucursalId,CompraId,Total,CarritoId,ClienteId")] CompraNueva compra)
         {
            Sucursal sucursal = _context.Sucursales.Include(c => c.StockItems).First(c => c.SucursalId == compra.SucursalId);
            List<CarritoItem> items = _context.Carritos.Include(c =>c.CarritoItems).First(c => c.ClienteId == compra.ClienteId && c.Activo).CarritoItems;
            if (!hayStock(sucursal, items))
            {
                List<Sucursal> sucursalesConStock = _context.Sucursales.Include(c => c.StockItems).ToList().Where(c => hayStock(c, items)).ToList();

                if (!sucursalesConStock.Any())
                {
                    return RedirectToAction("Details", "Carritos", new { id = compra.ClienteId , msg = "No hay sucursales con stock de los productos seleccionados" });
                }

                ModelState.AddModelError(string.Empty, "Dicha sucursal no tiene stock de todos los productos pedidos");
                ViewData["TotalValue"] = 0;
                ViewData["ClienteId"] = compra.ClienteId;
                ViewData["CarritoId"] = _context.Carritos.First(c => c.ClienteId == compra.ClienteId && c.Activo).CarritoId;
                ViewData["Sucursales"] = new SelectList(sucursalesConStock, "SucursalId", "Nombre");
                return View(compra);
            }

            if (ModelState.IsValid)
             {
                Sucursal sucursalNueva = _context.Sucursales.Find(compra.SucursalId);
                //var carrito = _context.Carritos.Include(c => c.CarritoItems).First(c => c.CarritoId == compra.CarritoId);
                if(sucursalNueva == null)
                {
                    ModelState.AddModelError(string.Empty, "Sucursal no encontrada");
                    return View(compra);
                }
                var carritoI = _context.CarritosItems.Include(c => c.Producto).Where(c => c.CarritoId == compra.CarritoId);
                if (carritoI != null)
                {
                    float total = 0;
                    foreach (var item in carritoI)
                    {
                        total += item.Subtotal;
                    }

                    Compra compraNueva = new Compra()
                    {
                        Total = total,
                        CarritoId = compra.CarritoId,
                        ClienteId = compra.ClienteId
                    };

                    desactivarCarrito(compra.CarritoId);
                    crearNuevoCarrito(compra.ClienteId);
                    eliminarItemsDeStock(sucursalNueva, items);

                    _context.Compras.Add(compraNueva);
                    await _context.SaveChangesAsync();
                    compra.Total = total;
                    compra.CompraId = compraNueva.CompraId;
                    compra.Fecha = DateTime.Now;
                    return RedirectToAction(nameof(Agradecimiento), compra);
                }
                                
             }
            ViewData["TotalValue"] = 0;
            ViewData["ClienteId"] = compra.ClienteId;
            ViewData["CarritoId"] = _context.Carritos.First(c => c.ClienteId == compra.ClienteId && c.Activo).CarritoId;
            ViewData["Sucursales"] = new SelectList(_context.Sucursales, "SucursalId", "Nombre");
            return View(compra);
         }

        private void eliminarItemsDeStock(Sucursal sucursalNueva, List<CarritoItem> items)
        {
            foreach(var item in items)
            {
                StockItem itemDeStock = _context.StocksItems.First(c => c.SucursalId == sucursalNueva.SucursalId && c.ProductoId == item.ProductoId);
                sucursalNueva.StockItems.Remove(itemDeStock);
            }
            
        }

        private bool hayStock(Sucursal sucursal, List<CarritoItem> items)
        {
            bool hayStock = true;
            int i = 0;

            while(i < items.Count && hayStock)
            {
                foreach(CarritoItem item in items)
                {
                    if(!sucursal.StockItems.Any(c => c.ProductoId == item.ProductoId))
                    {
                        hayStock = false;
                        break;
                    }
                    i++;
                }
            }


            return hayStock;
        }

        private void crearNuevoCarrito(int id)
        {
            Carrito carritoNuevo = new Carrito(id);
            _context.Carritos.Add(carritoNuevo);
        }

        private void desactivarCarrito(int carritoId)
        {
            _context.Carritos.Find(carritoId).Activo = false;
        }

        /*
         * NO SE ELIMINAN ITEMS DE CARRITO SINO QUE SE DESACTIVA EL CARRITO
        private void eliminoItemsDeCarrito(int carritoId)
        {
            var carrito = _context.Carritos.Include(c => c.CarritoItems).First(c => c.CarritoId == carritoId);

            carrito.CarritoItems.RemoveAll(c => c.CarritoId == carritoId);
        }
        */

        public IActionResult Agradecimiento(CompraNueva compra)
        {
            ViewData["Mensaje"] = "Muchas gracias por confiar en nosotros!";
            ViewData["Sucursal"] = _context.Sucursales.Find(compra.SucursalId);

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
