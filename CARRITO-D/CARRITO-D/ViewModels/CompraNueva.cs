using CARRITO_D.Helpers;
using CARRITO_D.Models;
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.ViewModels
{
    public class CompraNueva
    {
        public int CompraId { get; set; }


        [Required]
        [DataType(DataType.Currency, ErrorMessage = ErrorMsg.TipoInvalido)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)] // CUANDO SE USAN DESCUENTOS PUEDE PASAR Q QUEDE EN 0
        public float Total { get; set; } = 1;

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        public int SucursalId { get; set; }
    }
}
