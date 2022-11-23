using CARRITO_D.Data;
using CARRITO_D.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CARRITO_D.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CarritoContext _context;

        public HomeController(ILogger<HomeController> logger, CarritoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            if(_context.Carritos.Any())
            {
                ViewData["CarritoId"] = _context.Carritos.First(c => c.Activo == true).CarritoId;
            }           

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}