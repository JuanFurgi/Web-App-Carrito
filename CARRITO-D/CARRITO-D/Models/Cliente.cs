
using System.ComponentModel.DataAnnotations;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class Cliente : Persona
    {

        [Required(ErrorMessage =ErrorMsg.MsgReq)]
        [Range(Restricciones.MinDNI, Restricciones.MaxDNI, ErrorMessage =ErrorMsg.MsgMinMaxRange)]
        public int DNI { get; set; }
        public List<Carrito> Carritos { get; set; }
        public List<Compra> Compras { get; set; }   //Historial de compras Tal vez   
        
    }
}
