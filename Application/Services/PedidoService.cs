using System;
using System.Collections.Generic;
using AV2.Domain.Entities;
using AV2.Domain.Interfaces;
using AV2.Domain.ValueObjects;
using AV2.Application.DTOs.PedidoDTOs;

namespace AV2.Application.Services
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
            var carrinho = _carrinhoRepo.ObterPorId(dto.IdCarrinho)
                ?? throw new Exception("Carrinho não encontrado.");

            // Criar um endereço padrão (ajuste conforme necessário)
            // OPÇÃO 1: Usar endereço do cliente
            var endereco = carrinho.Cliente.Endereco 
                ?? Endereco.Create(
                    logradouro: "Rua Padrão",
                    numero: "S/N",
                    complemento: "",
                    bairro: "Centro",
                    cidade: "Cidade",
                    estado: "UF",
                    cep: "00000000",
                    pais: "Brasil"
                );

            // Usar o método factory estático
            var pedido = Pedido.Create(carrinho.Cliente, carrinho, endereco);

            _pedidoRepo.Adicionar(pedido);

            return new PedidoResponseDTO
            {
                IdPedido = pedido.IdPedido,
                Status = pedido.Status.ToString(),
                Total = pedido.TotalDecimal
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

        public void AtualizarStatus(int idPedido, string statusStr)
        {
            var pedido = _pedidoRepo.ObterPorId(idPedido)
                ?? throw new Exception("Pedido não encontrado.");

            // Converter string para enum e usar os métodos apropriados
            if (Enum.TryParse<StatusPedido>(statusStr, true, out var novoStatus))
            {
                switch (novoStatus)
                {
                    case StatusPedido.PagamentoConfirmado:
                        pedido.ConfirmarPagamento();
                        break;
                    case StatusPedido.EmSeparacao:
                        pedido.IniciarSeparacao();
                        break;
                    case StatusPedido.EmTransporte:
                        pedido.IniciarEnvio();
                        break;
                    case StatusPedido.Entregue:
                        pedido.Entregar();
                        break;
                    case StatusPedido.Cancelado:
                        pedido.Cancelar("Cancelado pelo usuário");
                        break;
                    default:
                        throw new ArgumentException($"Transição para status {statusStr} não é permitida diretamente.");
                }

                _pedidoRepo.Atualizar(pedido);
            }
            else
            {
                throw new ArgumentException($"Status inválido: {statusStr}");
            }
        }
    }
}