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
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signInManager, CarritoContext context, IWebHostEnvironment hostingEnvironment)
        {
            this._userManager = usermanager;
            this._signInManager = signInManager;
            this._context = context;
            this._hostingEnvironment = hostingEnvironment;
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
                

                
                if(_context.Personas.Any(c => c.NormalizedEmail == viewmodel.Email.ToUpper()))
                {
                    ModelState.AddModelError(String.Empty, "Ya hay un cliente con ese Email registrado, pruebe con otro");
                }
                else
                {
                     
                    var resultadoCreate = await _userManager.CreateAsync(clienteNuevo, viewmodel.Password);

                    if (resultadoCreate.Succeeded)
                    {
                        Carrito carritoNuevo = new Carrito(clienteNuevo.Id);

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
                return RedirectToAction("Details", "Clientes", new { id = personaId });
            }
            else
            {
                return RedirectToAction("Details", "Empleados", new { id = personaId });
            }
            
        }


        #endregion

        #region Subir Foto

        [HttpGet]
        public IActionResult SubirFoto()
        {
            return View(new Representacion());
        }

        [HttpPost]
        public async Task<IActionResult> SubirFoto(Representacion modelo)
        {
            var Persona = await _userManager.GetUserAsync(User);
            string rootPath = _hostingEnvironment.WebRootPath;
            string fotoPath = Configs.FotoPATH;
            string userName = User.Identity.Name;

            if (ModelState.IsValid)
            {
                if(modelo.Imagen != null && Persona != null)
                {
                    string nombreArchivoUnico = null;

                    if(!string.IsNullOrEmpty(rootPath) && !string.IsNullOrEmpty(fotoPath) && modelo.Imagen != null)
                    {
                        try
                        {
                            string carpetaDestino = Path.Combine(rootPath, fotoPath);

                            //Verifico si es para un usuario o por sistema
                            nombreArchivoUnico = Guid.NewGuid().ToString() + (!string.IsNullOrEmpty(userName) ? "_" + userName : "_" + "Sistema") + "_"+ modelo.Imagen.FileName;

                            string rutaCompletaArchivo = Path.Combine(carpetaDestino, nombreArchivoUnico);

                            modelo.Imagen.CopyTo(new FileStream(rutaCompletaArchivo, FileMode.Create));
                            Persona.Foto = nombreArchivoUnico;

                            if (!string.IsNullOrEmpty(Persona.Foto))
                            {
                                _context.Personas.Update(Persona);
                                _context.SaveChanges();
                                return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> EliminarFoto(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Persona persona;

            if(id != 0)
            {
                persona = _context.Personas.Find(id);
            }
            else
            {
                persona = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if(persona != null)
            {
                if(persona.Foto != null)
                { 
                    string nuevoNombre = Configs.FotoDef;
                    persona.Foto = nuevoNombre;
                    _context.Personas.Update(persona);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }


        #endregion

        public IActionResult AccesoDenegado(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        

       
    }
}
