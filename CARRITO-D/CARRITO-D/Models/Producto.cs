using CARRITO_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CARRITO_D.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public Boolean Activo { get; set; }

        [StringLength(25, MinimumLength = 2, ErrorMessage =ErrorMsg.MsgMaxMinStr)]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        [DataType(DataType.Currency, ErrorMessage =ErrorMsg.TipoInvalido)]
        [Range(1, int.MaxValue, ErrorMessage =ErrorMsg.MsgMinMaxRange)]
        [Display(Name = Alias.ValorUnidad)]
        public float PrecioVigente { get; set; }  //usa 32 bits de espacio en vez de los 64 de double. Optimizacion de memoria

        //LE PONEMOS UNA RELACION CON CATEGORIA, UN PRODUCTO DEBE TENER UNA CATEGORIA
        [Required(ErrorMessage =ErrorMsg.MsgReq)]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public List<CarritoItem> CarritosItem { get; set; }

        public List<StockItem> StocksItem { get; set; }

    }
}
