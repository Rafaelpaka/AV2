using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly ICarrinhoRepository _carrinhoRepo;

        public PedidoService(IPedidoRepository pedidoRepo, ICarrinhoRepository carrinhoRepo)
        {
            _pedidoRepo = pedidoRepo;
            _carrinhoRepo = carrinhoRepo;
        }

        public PedidoResponseDTO CriarPedido(PedidoCreateDTO dto)
        {
            var carrinho = _carrinhoRepo.ObterPorId(dto.IdCarrinho);

            var pedido = new Pedido
            {
                Cliente = carrinho.Cliente,
                Status = "Aguardando Pagamento",
                Total = carrinho.Total
            };

            _pedidoRepo.Adicionar(pedido);

            return new PedidoResponseDTO
            {
                IdPedido = pedido.IdPedido,
                Status = pedido.Status,
                Total = pedido.Total
            };
        }


        public Pedido ObterPorId(int idPedido)
        {
            return _pedidoRepo.ObterPorId(idPedido);
        }

        public IEnumerable<Pedido> ObterPorCliente(int idCliente)
        {
            return _pedidoRepo.ObterPorCliente(idCliente);
        }

        public void AtualizarStatus(int idPedido, string status)
        {
            var pedido = _pedidoRepo.ObterPorId(idPedido);
            pedido.Status = status;
            _pedidoRepo.Atualizar(pedido);
        }
    }
}
