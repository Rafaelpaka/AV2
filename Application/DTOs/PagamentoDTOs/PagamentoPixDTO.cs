namespace AV2.Application.DTOs.PagamentoDTOs
{
    public class PagamentoPixDTO
    {
        public int IdPedido { get; set; }
        public decimal Valor { get; set; }
        public required string ChavePix { get; set; }
    }
}