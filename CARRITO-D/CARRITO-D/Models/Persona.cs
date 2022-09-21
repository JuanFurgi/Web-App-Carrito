using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.Models
{
    public abstract class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [MaxLength(35, ErrorMessage = ErrorMsg.MsgMaxStr)]
        [MinLength(2, ErrorMessage = ErrorMsg.MsgMinStr)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [MaxLength(35, ErrorMessage = ErrorMsg.MsgMaxStr)]
        public string UserName { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [MaxLength(25)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [EmailAddress(ErrorMessage = ErrorMsg.TipoInvalido)]
        public string Email { get; set; }
        public string Direccion { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set;}
        public int Telefono { get; set; } 

    }
}
