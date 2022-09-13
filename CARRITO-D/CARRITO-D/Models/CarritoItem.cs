﻿namespace CARRITO_D.Models
{
    public class CarritoItem {
        public int CarritoItemId { get; set; }
        public Carrito Carrito { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public float ValorUnitario { get; set; }
        public int Cantidad { get; set; }
       
    }
}
