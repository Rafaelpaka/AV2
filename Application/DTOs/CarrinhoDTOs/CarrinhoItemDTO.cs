namespace AV2.Application.DTOs.CarrinhoDTOs
{
    public class CarrinhoItemDTO
    {
        public int IdProduto { get; set; }
        public required string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}