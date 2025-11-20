namespace Application.DTOs.ProdutoDTOs
{
    public class ProdutoCreateDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; }
        public int Estoque { get; set; }
    }
}