using System;
using AV2.Domain.Entities;
using AV2.Domain.Interfaces;
using AV2.Domain.ValueObjects;
using AV2.Application.DTOs.PagamentoDTOs;

namespace AV2.Application.Services
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
            var pedido = _pedidoRepo.ObterPorId(idPedido)
                ?? throw new Exception("Pedido não encontrado.");

            
            var valor = Dinheiro.Create(pedido.TotalDecimal);

            
            var pagamento = PagamentoCartao.Create(
                valor,
                dto.NumeroCartao,
                dto.NomeTitular,
                dto.Validade,
                dto.Cvv,
                dto.Parcelas
            );

            
            pagamento.Processar();
            pagamento.Aprovar();
            
            
            pagamento.DefinirCodigoAutorizacao(Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper());

           
            pedido.DefinirPagamento(pagamento);
            pedido.ConfirmarPagamento();

            _pagamentoRepo.Registrar(pagamento);
            _pedidoRepo.Atualizar(pedido);

            return pagamento;
        }

        public PagamentoPix PagarComPix(int idPedido, PagamentoPixDTO dto)
        {
            var pedido = _pedidoRepo.ObterPorId(idPedido)
                ?? throw new Exception("Pedido não encontrado.");

            
            var valor = Dinheiro.Create(pedido.TotalDecimal);

            
            var pagamento = PagamentoPix.Create(
                valor,
                dto.ChavePix,
                30 
            );

            // Gerar QR Code (simulado - em produção viria de uma API)
            var qrCodeSimulado = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var codigoPixSimulado = Guid.NewGuid().ToString("N").ToUpper();
            pagamento.GerarQRCode(qrCodeSimulado, codigoPixSimulado);

            // Processar o pagamento
            pagamento.Processar();

            // Associar pagamento ao pedido
            pedido.DefinirPagamento(pagamento);

            _pagamentoRepo.Registrar(pagamento);
            _pedidoRepo.Atualizar(pedido);

            return pagamento;
        }

        public void AtualizarStatus(int idPagamento, string statusStr)
        {
            var pagamento = _pagamentoRepo.ObterPorId(idPagamento)
                ?? throw new Exception("Pagamento não encontrado.");

           
            if (Enum.TryParse<StatusPagamento>(statusStr, true, out var novoStatus))
            {
                
                switch (novoStatus)
                {
                    case StatusPagamento.Processando:
                        pagamento.Processar();
                        break;
                    case StatusPagamento.Aprovado:
                        pagamento.Aprovar();
                        break;
                    case StatusPagamento.Recusado:
                        pagamento.Recusar("Recusado pelo gateway");
                        break;
                    case StatusPagamento.Cancelado:
                        pagamento.Cancelar();
                        break;
                    case StatusPagamento.Estornado:
                        pagamento.Estornar();
                        break;
                    default:
                        throw new ArgumentException($"Status {statusStr} não pode ser aplicado diretamente.");
                }

              
                _pagamentoRepo.AtualizarStatus(idPagamento, novoStatus.ToString());
            }
            else
            {
                throw new ArgumentException($"Status inválido: {statusStr}");
            }
        }
    }
}