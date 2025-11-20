using System;
using Domain.ValueObjects;
using Domain.Exceptions;

namespace Domain.Entities
{
    public abstract class Pagamento
    {
        public int IdPagamento { get; protected set; }
        public Dinheiro Valor { get; protected set; }
        public StatusPagamento Status { get; protected set; }
        public DateTime DataCriacao { get; protected set; }
        public DateTime? DataProcessamento { get; protected set; }
        public string MensagemErro { get; protected set; }

        protected Pagamento()
        {
            DataCriacao = DateTime.Now;
            Status = StatusPagamento.Pendente;
        }

        public abstract string ObterTipoPagamento();

        public void Processar()
        {
            if (Status != StatusPagamento.Pendente)
                throw new OperacaoNaoPermitidaException($"Pagamento já foi processado com status: {Status}");

            Status = StatusPagamento.Processando;
        }

        public void Aprovar()
        {
            if (Status != StatusPagamento.Processando)
                throw new TransicaoStatusInvalidaException(Status.ToString(), StatusPagamento.Aprovado.ToString());

            Status = StatusPagamento.Aprovado;
            DataProcessamento = DateTime.Now;
        }

        public void Recusar(string motivo)
        {
            if (Status == StatusPagamento.Aprovado)
                throw new OperacaoNaoPermitidaException("Pagamento já foi aprovado e não pode ser recusado.");

            Status = StatusPagamento.Recusado;
            MensagemErro = motivo;
            DataProcessamento = DateTime.Now;

            throw new PagamentoRecusadoException(IdPagamento, motivo);
        }

        public void Cancelar()
        {
            if (Status == StatusPagamento.Aprovado)
                throw new OperacaoNaoPermitidaException("Pagamento aprovado não pode ser cancelado. Use estorno.");

            Status = StatusPagamento.Cancelado;
            DataProcessamento = DateTime.Now;
        }

        public void Estornar()
        {
            if (Status != StatusPagamento.Aprovado)
                throw new OperacaoNaoPermitidaException("Apenas pagamentos aprovados podem ser estornados.");

            Status = StatusPagamento.Estornado;
        }

        // Propriedade auxiliar para EF
        public decimal ValorDecimal => Valor?.Valor ?? 0;
    }

    public enum StatusPagamento
    {
        Pendente = 1,
        Processando = 2,
        Aprovado = 3,
        Recusado = 4,
        Cancelado = 5,
        Estornado = 6
    }
}