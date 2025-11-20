using System;
using Domain.ValueObjects;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Produto
    {
        public int IdProduto { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public Dinheiro Preco { get; private set; }
        public string Categoria { get; private set; }
        public int Estoque { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCriacao { get; private set; }

        // Construtor privado
        private Produto()
        {
            DataCriacao = DateTime.Now;
            Ativo = true;
        }

        // Factory Method
        public static Produto Create(string nome, string descricao, Dinheiro preco, 
                                     string categoria, int estoqueInicial)
        {
            var produto = new Produto();
            produto.AlterarNome(nome);
            produto.AlterarDescricao(descricao);
            produto.AlterarPreco(preco);
            produto.AlterarCategoria(categoria);
            produto.AdicionarEstoque(estoqueInicial);

            return produto;
        }

        // Métodos de comportamento
        public void AlterarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ValidacaoException(nameof(Nome), "Nome não pode ser vazio.");

            if (nome.Length < 3)
                throw new ValidacaoException(nameof(Nome), "Nome deve ter no mínimo 3 caracteres.");

            Nome = nome.Trim();
        }

        public void AlterarDescricao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ValidacaoException(nameof(Descricao), "Descrição não pode ser vazia.");

            Descricao = descricao.Trim();
        }

        public void AlterarPreco(Dinheiro novoPreco)
        {
            if (novoPreco == null)
                throw new ArgumentNullException(nameof(novoPreco));

            if (novoPreco.Valor <= 0)
                throw new ValidacaoException(nameof(Preco), "Preço deve ser maior que zero.");

            Preco = novoPreco;
        }

        public void AlterarCategoria(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
                throw new ValidacaoException(nameof(Categoria), "Categoria não pode ser vazia.");

            Categoria = categoria.Trim();
        }

        public void AdicionarEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ValidacaoException(nameof(Estoque), "Quantidade deve ser positiva.");

            Estoque += quantidade;
        }

        public void RemoverEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ValidacaoException(nameof(Estoque), "Quantidade deve ser positiva.");

            if (quantidade > Estoque)
                throw new ProdutoSemEstoqueException(IdProduto, quantidade, Estoque);

            Estoque -= quantidade;
        }

        public bool TemEstoqueDisponivel(int quantidade = 1)
        {
            return Estoque >= quantidade && Ativo;
        }

        public void Desativar()
        {
            if (!Ativo)
                throw new OperacaoNaoPermitidaException("Produto já está desativado.");
            
            Ativo = false;
        }

        public void Reativar()
        {
            if (Ativo)
                throw new OperacaoNaoPermitidaException("Produto já está ativo.");
            
            Ativo = true;
        }

        // Método auxiliar para Entity Framework
        public decimal PrecoDecimal => Preco?.Valor ?? 0;
    }
}