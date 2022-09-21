using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CARRITO_D.Models
{
    public class StockItem
    {
        [Key, ForeignKey("Sucursal")]
        public int Id { get; set; }
        public int Cantidad { get; set; }

  //LO SACAMOS PARA HACER UNA RELACION DE QUE UN STOCKITEM DEBE TENER UNA SUCURSAL
        //public int SucursalId { get; set; }
        public Sucursal Sucursal { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
    }
}
