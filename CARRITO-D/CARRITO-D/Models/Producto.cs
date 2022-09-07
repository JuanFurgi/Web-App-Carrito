namespace CARRITO_D.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public Boolean Activo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int PrecioVigente { get; set; }
        public Categoria Categoria { get; set; }
            

    }
}
