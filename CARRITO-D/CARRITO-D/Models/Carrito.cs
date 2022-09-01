namespace CARRITO_D.Models
{
    public class Carrito {

        public int Id { get; set; }
        
        public int Activo { get; set; }

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; }

        public List<CarritoItem> CarritoItems { get; set; }

        public int SubTotal { get; set; }
    }
}
