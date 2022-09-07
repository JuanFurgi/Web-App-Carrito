using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.Models
{
    public class Cliente : Persona
    {
        public int UserId { get; set; }
        public Usuario Usuario { get; set; }

        public List<Carrito> Carritos { get; set; }
        public List<Compra> Compras { get; set; }      
        
    }
}
