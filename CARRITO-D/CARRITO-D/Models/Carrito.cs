﻿using System.ComponentModel.DataAnnotations;
using CARRITO_D.Helpers;

namespace CARRITO_D.Models
{
    public class Carrito {

        public int CarritoId { get; set; }

        [Required(ErrorMessage =ErrorMsg.MsgReq)]
        public Boolean Activo { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.Currency, ErrorMessage = ErrorMsg.TipoInvalido)]
        [Range(0, int.MaxValue, ErrorMessage =ErrorMsg.MsgMinMaxRange)]
        public float Subtotal { get; set; }

        public List<CarritoItem> CarritoItems { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
