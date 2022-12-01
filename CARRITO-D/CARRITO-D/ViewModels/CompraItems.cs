using CARRITO_D.Models;
using NuGet.DependencyResolver;

namespace CARRITO_D.ViewModels
{
    public class CompraItems
    {
        public Compra Compra { get; set; }
        public IEnumerable<CarritoItem> Items { get; set; }
    }
}
