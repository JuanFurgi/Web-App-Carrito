namespace CARRITO_D.Models
{
    public class Carrito {

        public int Id { get; set; }
        
        private int Activo { get; set; }

        private int ClienteId { get; set; }

        private Cliente Cliente { get; set; }

        private List<CarritoItem> CarritoItems { get; set; }

        private int SubTotal { get; set; }
    }
}
