using System;
using System.Collections.Generic;
using System.Linq;
using AV2.Domain.ValueObjects;
using AV2.Domain.Exceptions;
using AV2.Domain.Events;

namespace AV2.Domain.Entities
{
    public class Pedido
    {
        public int IdPedido { get; private set; }
        public int IdCliente { get; private set; }
        public Cliente Cliente { get; private set; }
        public StatusPedido Status { get; private set; }
        public DateTime DataPedido { get; private set; }
        public DateTime? DataFinalizacao { get; private set; }
        public Endereco EnderecoEntrega { get; private set; }

        private readonly List<ItemPedido> _itens = new List<ItemPedido>();
        public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

        public int? IdPagamento { get; private set; }
        public Pagamento Pagamento { get; private set; }

        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private Pedido()
        {
            DataPedido = DateTime.Now;
            Status = StatusPedido.AguardandoPagamento;
        }

        public static Pedido Create(Cliente cliente, Carrinho carrinho, Endereco enderecoEntrega)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            if (carrinho == null)
                throw new ArgumentNullException(nameof(carrinho));

            if (enderecoEntrega == null)
                throw new ArgumentNullException(nameof(enderecoEntrega));

            carrinho.ValidarParaFinalizacao();

            var pedido = new Pedido
            {
                IdCliente = cliente.IdCliente,
                Cliente = cliente,
                EnderecoEntrega = enderecoEntrega
            };

            foreach (var itemCarrinho in carrinho.Itens)
            {
                pedido._itens.Add(ItemPedido.CreateFromCarrinho(itemCarrinho));
            }

            pedido.AdicionarEvento(new PedidoCriadoEvent(
                pedido.IdPedido,
                pedido.IdCliente,
                pedido.CalcularTotal().Valor
            ));

            return pedido;
        }

        public void DefinirPagamento(Pagamento pagamento)
        {
            if (pagamento == null)
                throw new ArgumentNullException(nameof(pagamento));

            if (Status != StatusPedido.AguardandoPagamento)
                throw new TransicaoStatusInvalidaException(Status.ToString(), "DefinirPagamento");

            Pagamento = pagamento;
            IdPagamento = pagamento.IdPagamento;
        }

        public void ConfirmarPagamento()
        {
            if (Status != StatusPedido.AguardandoPagamento)
                throw new TransicaoStatusInvalidaException(Status.ToString(), StatusPedido.PagamentoConfirmado.ToString());

            if (Pagamento == null)
                throw new OperacaoNaoPermitidaException("Pagamento não foi definido.");

            Status = StatusPedido.PagamentoConfirmado;
            
            AdicionarEvento(new PagamentoProcessadoEvent(
                Pagamento.IdPagamento,
                IdPedido,
                "Confirmado",
                Pagamento.Valor.Valor
            ));
        }

        public void IniciarSeparacao()
        {
            if (Status != StatusPedido.PagamentoConfirmado)
                throw new TransicaoStatusInvalidaException(Status.ToString(), StatusPedido.EmSeparacao.ToString());

            foreach (var item in _itens)
            {
                item.Produto.RemoverEstoque(item.Quantidade);
            }

            Status = StatusPedido.EmSeparacao;
        }

        public void IniciarEnvio()
        {
            if (Status != StatusPedido.EmSeparacao)
                throw new TransicaoStatusInvalidaException(Status.ToString(), StatusPedido.EmTransporte.ToString());

            Status = StatusPedido.EmTransporte;
        }

        public void Entregar()
        {
            if (Status != StatusPedido.EmTransporte)
                throw new TransicaoStatusInvalidaException(Status.ToString(), StatusPedido.Entregue.ToString());

            Status = StatusPedido.Entregue;
            DataFinalizacao = DateTime.Now;

            AdicionarEvento(new PedidoFinalizadoEvent(IdPedido, IdCliente));
        }

        public void Cancelar(string motivo)
        {
            if (Status == StatusPedido.Entregue)
                throw new OperacaoNaoPermitidaException("Pedido já foi entregue e não pode ser cancelado.");

            if (Status == StatusPedido.Cancelado)
                throw new OperacaoNaoPermitidaException("Pedido já está cancelado.");

            if (Status == StatusPedido.EmSeparacao || Status == StatusPedido.EmTransporte)
            {
                foreach (var item in _itens)
                {
                    item.Produto.AdicionarEstoque(item.Quantidade);
                }
            }

            Status = StatusPedido.Cancelado;
            DataFinalizacao = DateTime.Now;

            AdicionarEvento(new PedidoCanceladoEvent(IdPedido, motivo));
        }

        public Dinheiro CalcularTotal()
        {
            if (!_itens.Any())
                return Dinheiro.Zero();

            var total = Dinheiro.Zero();
            foreach (var item in _itens)
            {
                total = total + item.CalcularSubtotal();
            }
            return total;
        }

        public bool PodeSerCancelado()
        {
            return Status != StatusPedido.Entregue && Status != StatusPedido.Cancelado;
        }

        private void AdicionarEvento(IDomainEvent evento)
        {
            _domainEvents.Add(evento);
        }

        public void LimparEventos()
        {
            _domainEvents.Clear();
        }

        public decimal TotalDecimal => CalcularTotal().Valor;
    }

    public enum StatusPedido
    {
        AguardandoPagamento = 1,
        PagamentoConfirmado = 2,
        EmSeparacao = 3,
        EmTransporte = 4,
        Entregue = 5,
        Cancelado = 6
    }

    public class ItemPedido
    {
        public int IdItemPedido { get; private set; }
        public int IdPedido { get; private set; }
        public int IdProduto { get; private set; }
        public Produto Produto { get; private set; }
        public string NomeProduto { get; private set; }
        public int Quantidade { get; private set; }
        public Dinheiro PrecoUnitario { get; private set; }

        private ItemPedido() { }

        public static ItemPedido CreateFromCarrinho(ItemCarrinho itemCarrinho)
        {
            return new ItemPedido
            {
                IdProduto = itemCarrinho.IdProduto,
                Produto = itemCarrinho.Produto,
                NomeProduto = itemCarrinho.Produto.Nome,
                Quantidade = itemCarrinho.Quantidade,
                PrecoUnitario = itemCarrinho.PrecoUnitario
            };
        }

        public Dinheiro CalcularSubtotal()
        {
            return PrecoUnitario * Quantidade;
        }

        public decimal PrecoUnitarioDecimal => PrecoUnitario?.Valor ?? 0;
    }
}