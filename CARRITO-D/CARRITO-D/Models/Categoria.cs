﻿namespace CARRITO_D.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }    
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<StockItem> Productos { get; set; }


    }
}
