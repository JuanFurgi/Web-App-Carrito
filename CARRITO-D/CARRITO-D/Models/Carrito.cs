namespace CARRITO_D.Models
{
    public class Carrito {
        public int CarritoId { get; set; }
        public Boolean Activo { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public List<CarritoItem> CarritoItems { get; set; }
        public int SubTotal { get; set; }
    }
}
