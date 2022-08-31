namespace CARRITO_D.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }  
        public DateOnly FechaAlta { get; set; } 

        private Telefono Telefono { get; set; } 

    }
}
