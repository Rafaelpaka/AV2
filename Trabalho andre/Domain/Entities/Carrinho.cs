using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Carrinho
    {
        public int IdCarrinho { get; set; }

        private readonly List<Produto> _itens = new List<Produto>();
        public IReadOnlyCollection<Produto> Itens => _itens.AsReadOnly();

        public void AdicionarProduto(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            if (produto.Estoque <= 0)
                throw new InvalidOperationException("Produto sem estoque disponível.");

            _itens.Add(produto);
        }

        public void RemoverProduto(Produto produto)
        {
            _itens.Remove(produto);
        }

        public decimal Total
        {
            get
            {
                return _itens.Sum(p => p.Preco);
            }
        }
    }
}
