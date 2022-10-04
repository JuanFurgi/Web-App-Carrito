using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CARRITO_D.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CARRITO_D.Models
{
    public class CarritoItem {

        [DataType(DataType.Currency, ErrorMessage =ErrorMsg.TipoInvalido)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)]
        public float Subtotal { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)] //PORQUE NO SABEMOS CUAL SERIA EL PRECIO MAX AL QUE UN PRODUCTO PODRIA LLEGAR A VALER
        [Display(Name = Alias.ValorUnidad)]
        public float ValorUnitario { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)] //PONEMOS 1 PORQUE CREEMOS QUE PARA ESTAR EN CARRITOITEM DEBE TENER POR LO MENOS 1
        public int Cantidad { get; set; }

        [Key, ForeignKey("Carrito")]
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        [Key, ForeignKey("Producto")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
    }
}
