using CARRITO_D.Helpers;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.Models
{
    public class Sucursal {

        [Display(Name = Alias.Sucursal)]
        public int SucursalId { get; set; }

        [StringLength(20, MinimumLength = 4, ErrorMessage =ErrorMsg.MsgMaxMinStr)]
        public string Nombre { get; set; }

        [StringLength(50, MinimumLength = 4, ErrorMessage = ErrorMsg.MsgMaxMinStr)]
        public string Direccion { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage =ErrorMsg.TipoInvalido)]
        public int Telefono { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = ErrorMsg.TipoInvalido)]
        public string Email { get; set; }
        public List<StockItem> StockItems { get; set; }
    }
}
