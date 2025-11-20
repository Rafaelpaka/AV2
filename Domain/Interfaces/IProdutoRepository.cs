using System.Collections.Generic;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Produto ObterPorId(int idProduto);
        IEnumerable<Produto> ObterTodos();
        IEnumerable<Produto> ObterPorCategoria(string categoria);
        void Adicionar(Produto produto);
        void Atualizar(Produto produto);
        void Remover(int idProduto);
    }
}
