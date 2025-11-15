using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepo;
        private readonly IPedidoRepository _pedidoRepo;

        public PagamentoService(IPagamentoRepository pagamentoRepo, IPedidoRepository pedidoRepo)
        {
            _pagamentoRepo = pagamentoRepo;
            _pedidoRepo = pedidoRepo;
        }

        public PagamentoCartao PagarComCartao(int idPedido, PagamentoCartao dados)
        {
            dados.Status = "Processando";
            _pagamentoRepo.Registrar(dados);

            return dados;
        }

        public PagamentoPix PagarComPix(int idPedido, PagamentoPix dados)
        {
            dados.Status = "Aguardando PIX";
            _pagamentoRepo.Registrar(dados);

            return dados;
        }

        public void AtualizarStatus(int idPagamento, string status)
        {
            _pagamentoRepo.AtualizarStatus(idPagamento, status);
        }
    }
}
