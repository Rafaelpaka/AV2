using System;
using System.Collections.Generic;
using System.Linq;
using AV2.Domain.ValueObjects;
using AV2.Domain.Exceptions;

namespace AV2.Domain.Entities
{
    public class Carrinho
    {
        public int IdCarrinho { get; private set; }
        public int IdCliente { get; private set; }
        public Cliente Cliente { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataExpiracao { get; private set; }

        private readonly List<ItemCarrinho> _itens = new List<ItemCarrinho>();
        public IReadOnlyCollection<ItemCarrinho> Itens => _itens.AsReadOnly();

        private Carrinho()
        {
            DataCriacao = DateTime.Now;
            DataExpiracao = DateTime.Now.AddDays(7);
        }

        public static Carrinho Create(int idCliente, Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            if (!cliente.PodeRealizarCompras())
                throw new OperacaoNaoPermitidaException("Cliente não pode realizar compras.");

            return new Carrinho
            {
                IdCliente = idCliente,
                Cliente = cliente
            };
        }

        public void AdicionarProduto(Produto produto, int quantidade = 1)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            if (EstaExpirado())
                throw new OperacaoNaoPermitidaException("Carrinho expirado.");

            if (!produto.TemEstoqueDisponivel(quantidade))
                throw new ProdutoSemEstoqueException(produto.IdProduto, quantidade, produto.Estoque);

            var itemExistente = _itens.FirstOrDefault(i => i.IdProduto == produto.IdProduto);

            if (itemExistente != null)
            {
                itemExistente.AdicionarQuantidade(quantidade);
            }
            else
            {
                _itens.Add(ItemCarrinho.Create(produto, quantidade));
            }
        }

        public void RemoverProduto(int idProduto)
        {
            if (EstaExpirado())
                throw new OperacaoNaoPermitidaException("Carrinho expirado.");

            var item = _itens.FirstOrDefault(i => i.IdProduto == idProduto);
            if (item == null)
                throw new EntidadeNaoEncontradaException("Produto", idProduto);

            _itens.Remove(item);
        }

        public void AlterarQuantidade(int idProduto, int novaQuantidade)
        {
            if (EstaExpirado())
                throw new OperacaoNaoPermitidaException("Carrinho expirado.");

            var item = _itens.FirstOrDefault(i => i.IdProduto == idProduto);
            if (item == null)
                throw new EntidadeNaoEncontradaException("Produto", idProduto);

            if (novaQuantidade <= 0)
            {
                RemoverProduto(idProduto);
            }
            else
            {
                item.AlterarQuantidade(novaQuantidade);
            }
        }

        public void Limpar()
        {
            _itens.Clear();
        }

        public Dinheiro CalcularTotal()
        {
            if (!_itens.Any())
                return Dinheiro.Zero();

            var total = Dinheiro.Zero();
            foreach (var item in _itens)
            {
                total = total + item.CalcularSubtotal();
            }
            return total;
        }

        public int QuantidadeTotal()
        {
            return _itens.Sum(i => i.Quantidade);
        }

        public bool EstaVazio()
        {
            return !_itens.Any();
        }

        public bool EstaExpirado()
        {
            return DataExpiracao.HasValue && DateTime.Now > DataExpiracao.Value;
        }

        public void ProrrogarExpiracao(int dias = 7)
        {
            DataExpiracao = DateTime.Now.AddDays(dias);
        }

        public void ValidarParaFinalizacao()
        {
            if (EstaVazio())
                throw new CarrinhoVazioException(IdCarrinho);

            if (EstaExpirado())
                throw new OperacaoNaoPermitidaException("Carrinho expirado.");

            foreach (var item in _itens)
            {
                if (!item.Produto.TemEstoqueDisponivel(item.Quantidade))
                    throw new ProdutoSemEstoqueException(item.IdProduto, item.Quantidade, item.Produto.Estoque);
            }
        }
    }

    public class ItemCarrinho
    {
        public int IdItemCarrinho { get; private set; }
        public int IdCarrinho { get; private set; }
        public int IdProduto { get; private set; }
        public Produto Produto { get; private set; }
        public int Quantidade { get; private set; }
        public Dinheiro PrecoUnitario { get; private set; }

        private ItemCarrinho() { }

        public static ItemCarrinho Create(Produto produto, int quantidade)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            if (quantidade <= 0)
                throw new ValidacaoException(nameof(Quantidade), "Quantidade deve ser maior que zero.");

            return new ItemCarrinho
            {
                IdProduto = produto.IdProduto,
                Produto = produto,
                Quantidade = quantidade,
                PrecoUnitario = produto.Preco
            };
        }

        public void AdicionarQuantidade(int quantidade)
        {
            if (quantidade <= 0)
                throw new ValidacaoException(nameof(Quantidade), "Quantidade deve ser positiva.");

            Quantidade += quantidade;
        }

        public void AlterarQuantidade(int novaQuantidade)
        {
            if (novaQuantidade <= 0)
                throw new ValidacaoException(nameof(Quantidade), "Quantidade deve ser maior que zero.");

            Quantidade = novaQuantidade;
        }

        public Dinheiro CalcularSubtotal()
        {
            return PrecoUnitario * Quantidade;
        }

        public decimal PrecoUnitarioDecimal => PrecoUnitario?.Valor ?? 0;
    }
}