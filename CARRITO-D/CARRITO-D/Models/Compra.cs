using CARRITO_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.Models
{
    public class Compra
    {
        public int CompraId { get; set; }


        [Required]
        [DataType(DataType.Currency, ErrorMessage = ErrorMsg.TipoInvalido)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)] // CUANDO SE USAN DESCUENTOS PUEDE PASAR Q QUEDE EN 0
        public float Total { get; set; } = 1;

        [DataType(DataType.Date, ErrorMessage = ErrorMsg.TipoInvalido)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        [Display(Name = Alias.FechaDeCompra)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }
    }
}
