
using Domain.Entities.


namespace Domain.Entities.Carrinho;

public class Carrinhos
{

	public int IdCarrinho { get; set; }

	public List<Produto> itens { get; set; } = new List<Produto>();

	public void AdicionarProduto(Produto produto)
	{
		Itens.Add(produto);
	}

	public void RemoverProduto(Produto produto)
	{

		itens.remove(produto);
	}

	public decimal Total { get; set; }{

	

}


}