namespace CARRITO_D.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public Boolean Activo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float PrecioVigente { get; set; }  //usa 32 bits de espacio en vez de los 64 de double. Optimizacion de memoria

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
            

    }
}
