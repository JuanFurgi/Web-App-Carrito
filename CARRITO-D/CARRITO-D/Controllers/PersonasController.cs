using Microsoft.AspNetCore.Mvc;

namespace CARRITO_D.Controllers
{
    public class PersonasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(string nombre, string apellido)
        {
            
        }
    }
}
