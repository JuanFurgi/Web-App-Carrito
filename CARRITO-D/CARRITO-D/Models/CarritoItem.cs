namespace CARRITO_D.Models
{
    public class CarritoItem {
        public int CarritoItemId { get; set; }

        public float Subtotal { get; set; }
        public float ValorUnitario { get; set; }
        public int Cantidad { get; set; }

        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
    }
}
