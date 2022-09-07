namespace CARRITO_D.Models
{
    public class Sucursal {
        public int SucursalId { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; }
        public List<StockItem> StockItems { get; set; }
    }
}
