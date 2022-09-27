using CARRITO_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.MsgRegExpression)]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<StockItem> Productos { get; set; }


    }
}
