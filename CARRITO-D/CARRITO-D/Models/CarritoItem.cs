namespace CARRITO_D.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        public Carrito Carrito { get; set; }

        private int ProductoId { get; set; }
        public Producto Producto { get; set; }

        private int ValorUnitario { get; set; }

        private int Cantidad { get; set; }

        private int SubTotal { get; set; }
    }
}
