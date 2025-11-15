namespace Domain.Interfaces
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
