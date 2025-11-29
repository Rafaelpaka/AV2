namespace AV2.Application.DTOs.ProdutoDTOs
{
    public class ProdutoCreateDTO
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public decimal Preco { get; set; }
        public required string Categoria { get; set; }
        public int Estoque { get; set; }
    }
}