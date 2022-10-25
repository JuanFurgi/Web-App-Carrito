using CARRITO_D.Data;
using CARRITO_D.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CARRITO_D
{
    public static class StartUp
    {
        public static WebApplication InicializarApp(string[] args)
        {
            //Crear una nueva instancia de nuestro servidor Web
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder); //lo configuramos, con sus respectivos servicios

            var app = builder.Build(); //sobre esta app configuraremos luego los middleware
            Configure(app); // Configuramos los middleware

            return app; //Retornamos la aplicacion ya inicializada
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            //Configurado el entorno aplicativo para tener un servicio de acceso a nuestra Base de Datos
            //builder.Services.AddDbContext<CarritoContext>(options => options.UseInMemoryDatabase("CarritoDb"));
            builder.Services.AddDbContext<CarritoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CarritoDBCS")));


            #region Identity

            builder.Services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<CarritoContext>();

            builder.Services.Configure<IdentityOptions>(opciones =>
            {
                opciones.Password.RequireDigit = false;
                opciones.Password.RequiredLength = 5;
            }
            );

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
            {
                opciones.LoginPath = "/Account/IniciarSesion";
                opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                opciones.Cookie.Name = "IdentidadCarritoApp";
            });
            //Password por defecto en pre-carga: Password1!

            /* Configuraciones por defecto para Password son
             *  opciones.Password.RequireDigit = true;
                opciones.Password.RequiredLength = 6;
             * 
             */

            #endregion

            // Add services to the container.
            builder.Services.AddControllersWithViews();
        }

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();


            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
