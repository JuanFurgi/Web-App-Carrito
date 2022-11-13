using System.ComponentModel.DataAnnotations;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class Carrito {

        public Carrito() { }

        /*public Carrito(Boolean activo, int clienteId)
        /{
            this.Activo = activo;
            this.Subtotal = 0;
            this.ClienteId = clienteId;
        }*/

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
                if (CarritoItems.Count() > 0)
                {
                    foreach (CarritoItem item in CarritoItems)
                    {
                        Subtotal += item.Subtotal;
                    }
                }
                else
                {
                    Subtotal = 0;
                }
                
            }
        }

        public List<CarritoItem> CarritoItems { get; set; }

        [Required(ErrorMessage =ErrorMsg.MsgReq)]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
