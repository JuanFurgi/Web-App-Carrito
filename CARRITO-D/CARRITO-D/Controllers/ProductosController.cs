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
using CARRITO_D.Helpers;
using CARRITO_D.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CARRITO_D.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly CarritoContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductosController(CarritoContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this._hostingEnvironment = hostingEnvironment;
        }

        // GET: Productos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewData["Categorias"] = _context.Categorias.ToList().DistinctBy(c => c.Nombre);
            var carritoContext = _context.Productos.Include(p => p.Categoria);
            return View(await carritoContext.ToListAsync());
        }

        // GET: Productos/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
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

        // GET: Productos/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("Id,CategoriaId,Activo,Nombre,Descripcion,PrecioVigente")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", producto.Id);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Activo,Nombre,Descripcion,PrecioVigente,CategoriaId,Foto")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            ViewData["Id"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", producto.Id);
            return View(producto);
        }

        /*
        // GET: Productos/Delete/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'CarritoContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        #region Subir Foto

        [HttpGet]
        [Authorize(Roles ="Empleado")]
        public IActionResult SubirFotoProducto(int? id)
        {
            var producto = _context.Productos.First(c => c.Id == id);
            if(producto != null)
            {
                ViewData["Producto"] = producto;
            }
            else
            {
                return NotFound();
            }

            return View(new Representacion());
        }

        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> SubirFotoProducto(int? id, Representacion modelo)
        {
            if(modelo.Imagen == null)
            {
                ModelState.AddModelError(string.Empty, "Ingrese una imagen antes");
                var producto = _context.Productos.First(c => c.Id == id);
                if (producto != null)
                {
                    ViewData["Producto"] = producto;
                }
                else
                {
                    return NotFound();
                }
                return View(modelo);
            }

            var Producto = await _context.Productos.FindAsync(id);
            string rootPath = _hostingEnvironment.WebRootPath;
            string fotoPath = Configs.ProductosPATH;
            string productoName = Producto.Nombre;

            if (ModelState.IsValid)
            {
                if (modelo.Imagen != null && Producto != null)
                {
                    string nombreArchivoUnico = null;

                    if (!string.IsNullOrEmpty(rootPath) && !string.IsNullOrEmpty(fotoPath) && modelo.Imagen != null)
                    {
                        try
                        {
                            string carpetaDestino = Path.Combine(rootPath, fotoPath);

                            //Verifico si es para un usuario o por sistema
                            nombreArchivoUnico = Guid.NewGuid().ToString() + (!string.IsNullOrEmpty(productoName) ? "_" + productoName : "_" + "Sistema") + "_" + modelo.Imagen.FileName;

                            string rutaCompletaArchivo = Path.Combine(carpetaDestino, nombreArchivoUnico);

                            modelo.Imagen.CopyTo(new FileStream(rutaCompletaArchivo, FileMode.Create));
                            Producto.Foto = nombreArchivoUnico;

                            if (!string.IsNullOrEmpty(Producto.Foto))
                            {
                                _context.Productos.Update(Producto);
                                _context.SaveChanges();
                                return RedirectToAction("Edit", "Productos", new {id = id});
                            }
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Error en el proceso de carga");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Error Datos Insuficientes");
                }
            }

            return View(modelo);
        }

        #endregion

        #region Eliminar Foto

        [HttpPost]
        public IActionResult EliminarFotoProducto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Producto producto;

            if (id != 0)
            {
                producto = _context.Productos.Find(id);
            }
            else
            {
                return NotFound();
            }

            if (producto != null)
            {
                if (producto.Foto != null)
                {
                    string nuevoNombre = Configs.FotoProdDef;
                    producto.Foto = nuevoNombre;
                    _context.Productos.Update(producto);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Edit", "Productos", new { id = id });
        }


        #endregion




        private bool ProductoExists(int id)
        {
          return _context.Productos.Any(e => e.Id == id);
        }

        public async Task<IActionResult> SumarProducto(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FirstAsync(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return RedirectToAction("Create", "CarritosItems", id);
        }
    }
}
