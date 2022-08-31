namespace CARRITO_D.Models
{
    public class Compra
    {
        private int ClienteId { get; set; } 
        public Cliente Cliente { get; set; }

        private int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        private int Total { get; set; }
    }
}
