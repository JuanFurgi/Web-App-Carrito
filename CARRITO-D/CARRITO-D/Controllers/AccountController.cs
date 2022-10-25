﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CARRITO_D.Models;
using CARRITO_D.ViewModels;
using System.Reflection.Metadata.Ecma335;
using CARRITO_D.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CARRITO_D.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;

        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signInManager)
        {
            this._userManager = usermanager;
            this._signInManager = signInManager;

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

                var resultadoCreate = await _userManager.CreateAsync(clienteNuevo, viewmodel.Password);

                if (resultadoCreate.Succeeded)
                {
                    var resultadoAddRole = await _userManager.AddToRoleAsync(clienteNuevo, Configs.ClienteRolName);

                    if (resultadoAddRole.Succeeded)
                    {
                        await _signInManager.SignInAsync(clienteNuevo, false);
                        return RedirectToAction("Edit", "Clientes", new { id = clienteNuevo.Id });
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, $"No se pudo agregar el rol de {Configs.ClienteRolName}");
                    }
                    
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

        [AllowAnonymous]
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

        #region Cerrar sesion

        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
