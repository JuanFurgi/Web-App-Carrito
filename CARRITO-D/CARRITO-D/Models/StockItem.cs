using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class StockItem
    {
        
        public int Cantidad { get; set; }

        [Required(ErrorMessage =ErrorMsg.MsgReq)]
        [Key]
        [Display(Name = Alias.SucursalId)]
        public int SucursalId { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Key]
        [Display(Name = Alias.ProductoId)]
        public int ProductoId { get; set; }

        public Sucursal Sucursal { get; set; }
        public Producto Producto { get; set; }


    }
}
