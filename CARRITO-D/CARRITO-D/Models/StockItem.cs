using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class StockItem
    {

        [Range(0, int.MaxValue, ErrorMessage =ErrorMsg.MsgMinMaxRange)]
        public int Cantidad { get; set; }

        [Required(ErrorMessage =ErrorMsg.MsgReq)]
        [Key]
        [Display(Name =Alias.Sucursal)]
        public int SucursalId { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Key]
        [Display(Name =Alias.Producto)]
        public int ProductoId { get; set; }

        public Sucursal Sucursal { get; set; }
        public Producto Producto { get; set; }


    }
}
