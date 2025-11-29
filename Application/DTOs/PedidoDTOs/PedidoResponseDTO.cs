using System;

namespace Application.DTOs.PedidoDTOs
{
    public class PedidoResponseDTO
    {
        public int IdPedido { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public DateTime DataPedido { get; set; }
    }
}