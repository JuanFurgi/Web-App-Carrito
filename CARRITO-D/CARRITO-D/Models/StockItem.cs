namespace CARRITO_D.Models
{
    public class StockItem
    {
        public int StockItemId { get; set; }

        public int SucursalId { get; set; }
        public Sucursal Sucursal { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }  

        public int Cantidad { get; set; }
    }
}
