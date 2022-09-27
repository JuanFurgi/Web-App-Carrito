using CARRITO_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.Models
{
    public class Empleado : Persona
    {
        [Required(ErrorMessage= ErrorMsg.MsgReq)]
        public int Legajo { get; set; }

    }
}
