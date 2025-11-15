using System;

namespace Domain.Entities
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
        public int IdCarrinho { get; set; }
        public Carrinho Carrinho { get; set; }
        public int? IdPagamento { get; set; }
        public Pagamento Pagamento { get; set; }

        public string Status { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal Total 
        {
            get
            {
                return Carrinho != null ? Carrinho.Total : 0;
            }
        }
        public Pedido() { }

        public Pedido(Cliente cliente, Carrinho carrinho)
        {
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            Carrinho = carrinho ?? throw new ArgumentNullException(nameof(carrinho));

            IdCliente = cliente.IdCliente;
            IdCarrinho = carrinho.IdCarrinho;

            Status = "Em Processamento";
            DataPedido = DateTime.Now;
        }

        public void DefinirPagamento(Pagamento pagamento)
        {
            Pagamento = pagamento ?? throw new ArgumentNullException(nameof(pagamento));
            IdPagamento = pagamento.IdPagamento;
        }

        public void AtualizarStatus(string novoStatus)
        {
            if (string.IsNullOrWhiteSpace(novoStatus))
                throw new ArgumentException("Status inválido.");

            Status = novoStatus;
        }
    }
}
