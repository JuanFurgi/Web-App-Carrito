using System.ComponentModel.DataAnnotations;
using CARRITO_D.Helpers;
using NuGet.Configuration;

namespace CARRITO_D.Models
{
    public class Carrito {

        public Carrito()
        {
            this.Activo = true;
       
            this.CarritoItems = new List<CarritoItem>();
        }

        public int CarritoId { get; set; }

        public Boolean Activo { get; set; }

        [DataType(DataType.Currency, ErrorMessage = ErrorMsg.TipoInvalido)]
        
        public float Subtotal
        {
            get
            {
                float resultado = 0;
                if (CarritoItems != null)
                {
                    
                    foreach(var item in CarritoItems)
                    {
                        resultado += item.Subtotal;
                    }
                }
                return resultado;
            }

        }

        public List<CarritoItem> CarritoItems { get; set; }

        [Required(ErrorMessage =ErrorMsg.MsgReq)]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
