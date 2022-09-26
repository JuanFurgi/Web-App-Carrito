using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace CARRITO_D.Models
{
    public class Usuario : Persona
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
