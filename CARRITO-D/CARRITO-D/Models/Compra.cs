﻿namespace CARRITO_D.Models
{
    public class Compra
    {
        public int CompraId { get; set; }

        public float Total { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }
    }
}