using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.Models
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [MaxLength(35, ErrorMessage = ErrorMsg.MsgMaxStr)]
        [MinLength(2, ErrorMessage = ErrorMsg.MsgMinStr)]
//PODRIAMOS PONER StringLength(35, MinumLength=2, ErrorMsg.MsgEntreStr)
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
