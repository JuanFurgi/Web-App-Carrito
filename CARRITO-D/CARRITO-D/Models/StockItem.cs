using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class StockItem
    {
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage =ErrorMsg.MsgMinMaxRange)]
        public int Cantidad { get; set; }

        [ForeignKey("Sucursal")]
        [Display(Name =Alias.Sucursal)]
        public int SucursalId { get; set; }

        [ForeignKey("Producto")]
        [Display(Name =Alias.Producto)]
        public int ProductoId { get; set; }

        public Sucursal Sucursal { get; set; }
        public Producto Producto { get; set; }


    }
}
