namespace CARRITO_D.Models
{
    public abstract class Persona
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaAlta { get; set;}
        public int Telefono { get; set; } 

    }
}
