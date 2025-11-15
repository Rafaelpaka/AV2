namespace Domain.Entities
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; }
        public int Estoque { get; set; }
    }
}
