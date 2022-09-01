namespace CARRITO_D.Models
{
    public class Compra
    {
        public int ClienteId { get; set; } 
        public Cliente Cliente { get; set; }

        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        public int Total { get; set; }
    }
}
