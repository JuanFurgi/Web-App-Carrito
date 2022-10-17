using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CARRITO_D.Models;
using CARRITO_D.ViewModels;

namespace CARRITO_D.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;

        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signInManager)
        {
            this._usermanager = usermanager;
            this._signInManager = signInManager;

        }
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistroUsuario viewmodel)
        {
            if (ModelState.IsValid)
            {
                Cliente clienteNuevo = new Cliente()
                {
                    Email = viewmodel.Email,
                    UserName = viewmodel.UserName,
                };

                var resultadoCreate = await _usermanager.CreateAsync(clienteNuevo, viewmodel.Password);

                if (resultadoCreate.Succeeded)
                {
                    await _signInManager.SignInAsync(clienteNuevo, false);
                    return RedirectToAction("Edit", "Clientes", new {id = clienteNuevo.Id});
                }

                foreach(var error in resultadoCreate.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }

            return View(viewmodel);
        }
    }
}
