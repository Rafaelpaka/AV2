namespace AV2.Application.DTOs.PagamentoDTOs
{
    public class PagamentoCartaoDTO
    {
        public int IdPedido { get; set; }
        public decimal Valor { get; set; }
        public required string NumeroCartao { get; set; }
        public required string NomeTitular { get; set; }
        public required string Validade { get; set; }
        public required string Cvv { get; set; }
        public int Parcelas { get; set; }
    }
}