using System;
using System.Linq;
using AV2.Domain.ValueObjects;
using AV2.Domain.Exceptions;

namespace AV2.Domain.Entities{

public class PagamentoCartao : Pagamento
    {
        public string NumeroCartaoMascarado { get; private set; }
        public string NomeTitular { get; private set; }
        public string Validade { get; private set; }
        public int Parcelas { get; private set; }
        public string CodigoAutorizacao { get; private set; }

        private PagamentoCartao() : base() { }

        public static PagamentoCartao Create(Dinheiro valor, string numeroCartao, string nomeTitular, 
                                            string validade, string cvv, int parcelas)
        {
            if (valor == null || valor.Valor <= 0)
                throw new ValidacaoException(nameof(Valor), "Valor deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(numeroCartao))
                throw new ValidacaoException(nameof(numeroCartao), "Número do cartão é obrigatório.");

            if (string.IsNullOrWhiteSpace(nomeTitular))
                throw new ValidacaoException(nameof(nomeTitular), "Nome do titular é obrigatório.");

            if (string.IsNullOrWhiteSpace(validade))
                throw new ValidacaoException(nameof(validade), "Validade é obrigatória.");

            if (string.IsNullOrWhiteSpace(cvv) || cvv.Length < 3)
                throw new ValidacaoException(nameof(cvv), "CVV inválido.");

            if (parcelas < 1 || parcelas > 12)
                throw new ValidacaoException(nameof(parcelas), "Parcelas deve estar entre 1 e 12.");

            
            if (!ValidarValidade(validade))
                throw new ValidacaoException(nameof(validade), "Cartão expirado ou validade inválida.");

            return new PagamentoCartao
            {
                Valor = valor,
                NumeroCartaoMascarado = MascararCartao(numeroCartao),
                NomeTitular = nomeTitular.Trim().ToUpper(),
                Validade = validade,
                Parcelas = parcelas
            };
        }

        public void DefinirCodigoAutorizacao(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Código de autorização inválido.");

            CodigoAutorizacao = codigo;
        }

        public Dinheiro ValorParcela()
        {
            return Valor / Parcelas;
        }

        private static string MascararCartao(string numero)
        {
           
            var numeroLimpo = new string(numero.Where(char.IsDigit).ToArray());
            
            if (numeroLimpo.Length < 4)
                return "****";

            var ultimosQuatro = numeroLimpo.Substring(numeroLimpo.Length - 4);
            return $"**** **** **** {ultimosQuatro}";
        }

        private static bool ValidarValidade(string validade)
        {
            
            var partes = validade.Split('/');
            if (partes.Length != 2)
                return false;

            if (!int.TryParse(partes[0], out int mes) || !int.TryParse(partes[1], out int ano))
                return false;

            if (mes < 1 || mes > 12)
                return false;

            
            if (ano < 100)
                ano += 2000;

            var dataValidade = new DateTime(ano, mes, 1).AddMonths(1).AddDays(-1);
            return dataValidade >= DateTime.Now;
        }

        public override string ObterTipoPagamento() => "Cartão de Crédito";
    }
}