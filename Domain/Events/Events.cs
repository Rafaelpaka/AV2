using System;

namespace AV2.Domain.Events
{
    // Interface base para todos os eventos de domínio
    public interface IDomainEvent
    {
        DateTime OcorridoEm { get; }
    }

    // Classe base abstrata
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime OcorridoEm { get; }

        protected DomainEvent()
        {
            OcorridoEm = DateTime.Now;
        }
    }

    // Eventos específicos

    public class PedidoCriadoEvent : DomainEvent
    {
        public int IdPedido { get; }
        public int IdCliente { get; }
        public decimal ValorTotal { get; }

        public PedidoCriadoEvent(int idPedido, int idCliente, decimal valorTotal)
        {
            IdPedido = idPedido;
            IdCliente = idCliente;
            ValorTotal = valorTotal;
        }
    }

    public class PagamentoProcessadoEvent : DomainEvent
    {
        public int IdPagamento { get; }
        public int IdPedido { get; }
        public string Status { get; }
        public decimal Valor { get; }

        public PagamentoProcessadoEvent(int idPagamento, int idPedido, string status, decimal valor)
        {
            IdPagamento = idPagamento;
            IdPedido = idPedido;
            Status = status;
            Valor = valor;
        }
    }

    public class ProdutoAdicionadoAoCarrinhoEvent : DomainEvent
    {
        public int IdCarrinho { get; }
        public int IdProduto { get; }
        public int Quantidade { get; }

        public ProdutoAdicionadoAoCarrinhoEvent(int idCarrinho, int idProduto, int quantidade)
        {
            IdCarrinho = idCarrinho;
            IdProduto = idProduto;
            Quantidade = quantidade;
        }
    }

    public class PedidoFinalizadoEvent : DomainEvent
    {
        public int IdPedido { get; }
        public int IdCliente { get; }

        public PedidoFinalizadoEvent(int idPedido, int idCliente)
        {
            IdPedido = idPedido;
            IdCliente = idCliente;
        }
    }

    public class EstoqueBaixoEvent : DomainEvent
    {
        public int IdProduto { get; }
        public string NomeProduto { get; }
        public int EstoqueAtual { get; }
        public int EstoqueMinimo { get; }

        public EstoqueBaixoEvent(int idProduto, string nomeProduto, int estoqueAtual, int estoqueMinimo)
        {
            IdProduto = idProduto;
            NomeProduto = nomeProduto;
            EstoqueAtual = estoqueAtual;
            EstoqueMinimo = estoqueMinimo;
        }
    }

    public class ClienteCriadoEvent : DomainEvent
    {
        public int IdCliente { get; }
        public string Nome { get; }
        public string Email { get; }

        public ClienteCriadoEvent(int idCliente, string nome, string email)
        {
            IdCliente = idCliente;
            Nome = nome;
            Email = email;
        }
    }

    public class PedidoCanceladoEvent : DomainEvent
    {
        public int IdPedido { get; }
        public string Motivo { get; }

        public PedidoCanceladoEvent(int idPedido, string motivo)
        {
            IdPedido = idPedido;
            Motivo = motivo;
        }
    }
}