using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CARRITO_D.Models;
using CARRITO_D.ViewModels;
using System.Reflection.Metadata.Ecma335;
using CARRITO_D.Helpers;
using Microsoft.AspNetCore.Authorization;
using CARRITO_D.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CARRITO_D.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly CarritoContext _context;

        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signInManager, CarritoContext context)
        {
            this._userManager = usermanager;
            this._signInManager = signInManager;
            this._context = context;
        }

        #region Registracion

        [AllowAnonymous]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(RegistroUsuario viewmodel)
        {
            if (ModelState.IsValid)
            {
                Cliente clienteNuevo = new Cliente()
                {
                    Email = viewmodel.Email,
                    UserName = viewmodel.UserName,
                };
                Carrito carritoNuevo = new Carrito(clienteNuevo.Id);

                
                if(_context.Personas.Any(c => c.NormalizedEmail == viewmodel.Email.ToUpper()))
                {
                    ModelState.AddModelError(String.Empty, "Ya hay un cliente con ese Email registrado, pruebe con otro");
                }
                else
                {
                     
                    var resultadoCreate = await _userManager.CreateAsync(clienteNuevo, viewmodel.Password);

                    if (resultadoCreate.Succeeded)
                    {
                        

                        var resultadoAddRole = await _userManager.AddToRoleAsync(clienteNuevo, Configs.ClienteRolName);

                        if (resultadoAddRole.Succeeded)
                        {
                            await _context.Carritos.AddAsync(carritoNuevo);
                            await _context.SaveChangesAsync();

                            await _signInManager.SignInAsync(clienteNuevo, false);
                            return RedirectToAction("Edit", "Clientes", new { id = clienteNuevo.Id });
                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, $"No se pudo agregar el rol de {Configs.ClienteRolName}");
                        }
                    }
                    foreach (var error in resultadoCreate.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }

                }                
            }

            return View(viewmodel);
        }

        #endregion

        #region Inicio de sesion

        [AllowAnonymous]
        public IActionResult IniciarSesion(string returnurl)
        {
            TempData["url"] = returnurl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(Login viewmodel)
        {
            var returnurl = TempData["url"] as string;

            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(viewmodel.UserName, viewmodel.Password, viewmodel.Recordarme, false);

                if (resultado.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnurl))
                    {
                        return Redirect(returnurl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Inicio de sesion invalido");
            }


            return View(viewmodel);
        }

        #endregion

        #region Cerrar sesion

        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Editar Perfil

        public IActionResult EditarMiPerfil()
        {

            //OBTENER NOMBRE DE USUARIO
            //string nombreUsuario = User.Identity.Name;

            //OBTENGO ID
            string personaId = _userManager.GetUserId(User);
            //OTRA FORMA
            // int personaId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (User.IsInRole("Cliente"))
            {
                return RedirectToAction("edit", "clientes", new { id = personaId });
            }
            else
            {
                return RedirectToAction("edit", "empleados", new { id = personaId });
            }
            
        }


        #endregion

        public IActionResult AccesoDenegado(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        

       
    }
}
