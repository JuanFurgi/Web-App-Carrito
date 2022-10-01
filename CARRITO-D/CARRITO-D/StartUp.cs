using CARRITO_D.Data;
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
            builder.Services.AddDbContext<CarritoContext>(options => options.UseInMemoryDatabase("CarritoDb"));

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
