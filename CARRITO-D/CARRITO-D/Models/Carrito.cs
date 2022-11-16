using System.ComponentModel.DataAnnotations;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class Carrito {

        public Carrito()
        {
            this.Activo = true;
            this.Subtotal = 0;
            this.CarritoItems = new List<CarritoItem>();
        }

        public int CarritoId { get; set; }

        public Boolean Activo { get; set; }

        [DataType(DataType.Currency, ErrorMessage = ErrorMsg.TipoInvalido)]
        [Range(0, int.MaxValue, ErrorMessage =ErrorMsg.MsgMinMaxRange)]
        public float Subtotal
        {
            get
            {
                return Subtotal;
            }
            set
            {
                Subtotal = value;
            }
        }

        public List<CarritoItem> CarritoItems { get; set; }

        [Required(ErrorMessage =ErrorMsg.MsgReq)]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
