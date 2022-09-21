using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CARRITO_D.Models
{
    public class Producto
    {
        [Key, ForeignKey("Categoria")]
        public int Id { get; set; }
        public Boolean Activo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float PrecioVigente { get; set; }  //usa 32 bits de espacio en vez de los 64 de double. Optimizacion de memoria

//LE PONEMOS UNA RELACION CON CATEGORIA, UN PRODUCTO DEBE TENER UNA CATEGORIA
        public Categoria Categoria { get; set; }
            

    }
}
