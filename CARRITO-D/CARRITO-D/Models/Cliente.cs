
using System.ComponentModel.DataAnnotations;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class Cliente : Persona
    {

        [Required(ErrorMessage =ErrorMsg.MsgReq)]
        [RegularExpression(@"[0-9]{2}\.[0-9]{3}\.[0-9]{3}", ErrorMessage = "El DNI debe tener un formato NN.NNN.NNN")]
        public int DNI { get; set; }
        public List<Carrito> Carritos { get; set; }
        public List<Compra> Compras { get; set; }   //Historial de compras Tal vez   
        
    }
}
