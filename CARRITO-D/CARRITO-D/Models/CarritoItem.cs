namespace CARRITO_D.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        public Carrito Carrito { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int ValorUnitario { get; set; }

        public int Cantidad { get; set; }

        public int SubTotal { get; set; }
    }
}
