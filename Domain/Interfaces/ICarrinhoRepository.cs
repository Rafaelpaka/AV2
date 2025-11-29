using System.Collections.Generic;
using AV2.Domain.Entities;

namespace AV2.Domain.Interfaces
{
    public interface ICarrinhoRepository
    {
        Carrinho ObterPorId(int idCarrinho);
        IEnumerable<Carrinho> ObterPorCliente(int idCliente);
        void Adicionar(Carrinho carrinho);
        void Atualizar(Carrinho carrinho);
        void Remover(int idCarrinho);
    }
}