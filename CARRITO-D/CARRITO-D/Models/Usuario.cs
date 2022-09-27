using System.ComponentModel.DataAnnotations;
using System.Numerics;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class Usuario : Persona
    {
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.Password, ErrorMessage =ErrorMsg.TipoInvalido)]
        public string Password { get; set; }
    }
}
