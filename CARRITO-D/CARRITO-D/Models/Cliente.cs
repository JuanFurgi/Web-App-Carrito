
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.Models
{
    public class Cliente : Persona
    {

//NO SE SI ESTA BIEN PONER LA RegExpression ASI
        [Required]
        [RegularExpression(@"[0-9]{8}", ErrorMessage = ErrorMsg.MsgMinStr)]
        public int DNI { get; set; }
        public List<Carrito> Carritos { get; set; }
        public List<Compra> Compras { get; set; }   //Historial de compras Tal vez   
        
    }
}
