using System.ComponentModel.DataAnnotations;
using CARRITO_D.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CARRITO_D.Models
{
    public class CarritoItem {
        public int CarritoItemId { get; set; }

        [DataType(DataType.Currency, ErrorMessage =ErrorMsg.TipoInvalido)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)]
        public float Subtotal { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)] //PORQUE NO SABEMOS CUAL SERIA EL PRECIO MAX AL QUE UN PRODUCTO PODRIA LLEGAR A VALER
        [Display(Name = Alias.ValorUnidad)]
        public float ValorUnitario { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)] //POR AHORA DEJAMOS MAX VALUE HASTA QUE SEPAMOS COMO RELACIONARLA CON EL STOCKITEM
        public int Cantidad { get; set; }

        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
    }
}
