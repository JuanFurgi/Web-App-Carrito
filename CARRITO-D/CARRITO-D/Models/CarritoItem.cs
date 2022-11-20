using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CARRITO_D.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CARRITO_D.Models
{
    public class CarritoItem {

        [Key]
        public int Id { get; set; }

        [DataType(DataType.Currency, ErrorMessage = ErrorMsg.TipoInvalido)]
        
        public float Subtotal 
        {
            get
            {
                return ValorUnitario * Cantidad;
            }        
        }

        
        [Display(Name = Alias.ValorUnidad)]
        [DataType(DataType.Currency, ErrorMessage = ErrorMsg.TipoInvalido)]
        public float ValorUnitario
        {
            get
            {
                float resultado = 0;
                if(Producto != null)
                {
                    resultado = Producto.PrecioVigente;
                }
                return resultado;
            }
        }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.MsgMinMaxRange)] //PONEMOS 1 PORQUE CREEMOS QUE PARA ESTAR EN CARRITOITEM DEBE TENER POR LO MENOS 1
        public int Cantidad { get; set; } = 0;

        [ForeignKey("Carrito")]
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        [ForeignKey("Producto")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
    }
}
