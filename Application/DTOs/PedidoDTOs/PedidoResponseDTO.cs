namespace AV2.Application.DTOs.PedidoDTOs
{
    public class PedidoResponseDTO
    {
        public int IdPedido { get; set; }
        public required string Status { get; set; }
        public decimal Total { get; set; }
    }
}