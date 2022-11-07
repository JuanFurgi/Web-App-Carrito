using CARRITO_D.Helpers;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.ViewModels
{
    public class RegistroUsuario
    {
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [EmailAddress(ErrorMessage = ErrorMsg.TipoInvalido)]
        public string Email { get; set;}

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [MaxLength(35, ErrorMessage = ErrorMsg.MsgMaxStr)]
        [MinLength(4, ErrorMessage = ErrorMsg.MsgMinStr)]
        [Display(Name = Alias.NombreDeUsuario)]
        public string UserName { get; set; }


        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.Password, ErrorMessage =ErrorMsg.TipoInvalido)]
        [MinLength(5, ErrorMessage =ErrorMsg.MsgMinStr)]
        [Display(Name =Alias.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.Password, ErrorMessage = ErrorMsg.TipoInvalido)]
        [MinLength(5, ErrorMessage = ErrorMsg.MsgMinStr)]
        [Display(Name = Alias.ConfirmarPassword)]
        [Compare("Password", ErrorMessage =ErrorMsg.IncPassword)]
        public string ConfirmacionPassword { get; set; }

    }
}
