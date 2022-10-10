using System.ComponentModel.DataAnnotations;
using System.Numerics;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class Usuario : Persona
    {
        //[Required(ErrorMessage = ErrorMsg.MsgReq)]
        //[DataType(DataType.Password, ErrorMessage =ErrorMsg.TipoInvalido)]
        //[MinLength(5, ErrorMessage =ErrorMsg.MsgMinStr)]
        //public string Password { get; set; }

        //HEREDA DE IdentityUser PasswordHash
    }
}
