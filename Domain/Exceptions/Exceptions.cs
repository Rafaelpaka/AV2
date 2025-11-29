using System;

namespace AV2.Domain.Exceptions
{
    // Exceção base do domínio
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }
        protected DomainException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    // Exceções específicas

    public class ProdutoSemEstoqueException : DomainException
    {
        public int IdProduto { get; }
        public int QuantidadeSolicitada { get; }
        public int EstoqueDisponivel { get; }

        public ProdutoSemEstoqueException(int idProduto, int quantidadeSolicitada, int estoqueDisponivel)
            : base($"Produto {idProduto} não possui estoque suficiente. Solicitado: {quantidadeSolicitada}, Disponível: {estoqueDisponivel}")
        {
            IdProduto = idProduto;
            QuantidadeSolicitada = quantidadeSolicitada;
            EstoqueDisponivel = estoqueDisponivel;
        }
    }

    public class CarrinhoVazioException : DomainException
    {
        public int IdCarrinho { get; }

        public CarrinhoVazioException(int idCarrinho)
            : base($"O carrinho {idCarrinho} está vazio e não pode ser finalizado.")
        {
            IdCarrinho = idCarrinho;
        }
    }

    public class PagamentoRecusadoException : DomainException
    {
        public int IdPagamento { get; }
        public string Motivo { get; }

        public PagamentoRecusadoException(int idPagamento, string motivo)
            : base($"Pagamento {idPagamento} foi recusado: {motivo}")
        {
            IdPagamento = idPagamento;
            Motivo = motivo;
        }
    }

    public class TransicaoStatusInvalidaException : DomainException
    {
        public string StatusAtual { get; }
        public string StatusNovo { get; }

        public TransicaoStatusInvalidaException(string statusAtual, string statusNovo)
            : base($"Não é possível mudar o status de '{statusAtual}' para '{statusNovo}'.")
        {
            StatusAtual = statusAtual;
            StatusNovo = statusNovo;
        }
    }

    public class EntidadeNaoEncontradaException : DomainException
    {
        public string TipoEntidade { get; }
        public int Id { get; }

        public EntidadeNaoEncontradaException(string tipoEntidade, int id)
            : base($"{tipoEntidade} com ID {id} não foi encontrado.")
        {
            TipoEntidade = tipoEntidade;
            Id = id;
        }
    }

    public class ValidacaoException : DomainException
    {
        public string Campo { get; }

        public ValidacaoException(string campo, string mensagem)
            : base($"Erro de validação no campo '{campo}': {mensagem}")
        {
            Campo = campo;
        }
    }

    public class PedidoJaFinalizadoException : DomainException
    {
        public int IdPedido { get; }

        public PedidoJaFinalizadoException(int idPedido)
            : base($"O pedido {idPedido} já foi finalizado e não pode ser modificado.")
        {
            IdPedido = idPedido;
        }
    }

    public class OperacaoNaoPermitidaException : DomainException
    {
        public OperacaoNaoPermitidaException(string mensagem)
            : base(mensagem) { }
    }
}