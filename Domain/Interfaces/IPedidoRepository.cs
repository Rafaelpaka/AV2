using System.Collections.Generic;
using AV2.Domain.Entities;
namespace AV2.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Pedido ObterPorId(int idPedido);
        IEnumerable<Pedido> ObterPorCliente(int idCliente);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);
        void Remover(int idPedido);
    }
}
