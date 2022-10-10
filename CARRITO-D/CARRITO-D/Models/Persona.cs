using System.ComponentModel.DataAnnotations;
using CARRITO_D.Helpers;
using Microsoft.AspNetCore.Identity;

namespace CARRITO_D.Models
{
    public class Persona : IdentityUser<int>
    {
        
        //public int Id { get; set; } LA HEREDA DE IdentityUser

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.MsgRegExpression)]
        [MaxLength(35, ErrorMessage = ErrorMsg.MsgMaxStr)]
        [MinLength(2, ErrorMessage = ErrorMsg.MsgMinStr)]
//PODRIAMOS PONER StringLength(35, MinumLength=2, ErrorMsg.MsgEntreStr)
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.MsgRegExpression)]
        [MaxLength(25, ErrorMessage = ErrorMsg.MsgMaxStr)]
        [MinLength(2, ErrorMessage = ErrorMsg.MsgMinStr)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [MaxLength(35, ErrorMessage = ErrorMsg.MsgMaxStr)]
        [MinLength(4, ErrorMessage = ErrorMsg.MsgMinStr)]
        [Display(Name = Alias.NombreDeUsuario)]
        public override string UserName { 
            get { return base.UserName; }
            set { base.UserName = value; }
        }


        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [EmailAddress(ErrorMessage = ErrorMsg.TipoInvalido)]
        public override string Email {
            get { return base.Email; }
            set { base.Email = value; }
        }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [MaxLength(25, ErrorMessage = ErrorMsg.MsgMaxStr)]
        [MinLength(5, ErrorMessage = ErrorMsg.MsgMinStr)]
        public string Direccion { get; set; }

        [DataType(DataType.Date,ErrorMessage = ErrorMsg.TipoInvalido)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = Alias.FechaDeCreacion)]
        public DateTime FechaAlta { get; set;}

        [DataType(DataType.PhoneNumber,ErrorMessage = ErrorMsg.TipoInvalido)]
        [Range(1000000000, 9999999999, ErrorMessage =ErrorMsg.TipoInvalido)]
        public int Telefono { get; set; } 

    }
}
