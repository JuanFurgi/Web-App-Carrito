using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CARRITO_D.Models;
using CARRITO_D.ViewModels;
using System.Reflection.Metadata.Ecma335;

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

        #region Registracion
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

        #endregion

        #region Inicio de sesion


        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewmodel)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(viewmodel.UserName, viewmodel.Password, viewmodel.Recordarme, false);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Inicio de sesion invalido");
            }


            return View(viewmodel);
        }

        #endregion

        #region Cerrrar sesion

        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
