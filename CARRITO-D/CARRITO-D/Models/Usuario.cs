using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace CARRITO_D.Models
{
    public class Usuario
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
