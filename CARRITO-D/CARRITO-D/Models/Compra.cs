using CARRITO_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.Models
{
    public class Compra
    {
        public int CompraId { get; set; }


        [DataType(DataType.Currency, ErrorMessage = ErrorMsg.TipoInvalido)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)] // CUANDO SE USAN DESCUENTOS PUEDE PASAR Q QUEDE EN 0
        public float Total
        {
            get
            {
                return Total;
            }
            set
            {
                Total = this.Carrito.Subtotal;
            }
        }
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }
    }
}
