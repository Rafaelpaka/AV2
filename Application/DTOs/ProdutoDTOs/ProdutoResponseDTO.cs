namespace AV2.Application.DTOs.ProdutoDTOs
{
    public class ProdutoResponseDTO
    {
        public int IdProduto { get; set; }
        public required string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}