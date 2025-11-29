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

        public PagamentoCartao PagarComCartao(int idPedido, PagamentoCartaoDTO dto)
        {
            var pag = new PagamentoCartao
            {
                NumeroCartao = dto.NumeroCartao,
                NomeTitular = dto.NomeTitular,
                Validade = dto.Validade,
                Cvv = dto.Cvv,
                Parcelas = dto.Parcelas,
                Status = "Processando"
            };

            _pagamentoRepo.Registrar(pag);
            return pag;
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
