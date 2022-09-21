using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace CARRITO_D.Models
{
    public class Usuario : Persona
    {   
        public string Password { get; set; }
    }
}
