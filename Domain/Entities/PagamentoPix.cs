public class PagamentoPix : Pagamento
    {
        public string ChavePix { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public string QRCode { get; private set; }
        public string CodigoPix { get; private set; }

        private PagamentoPix() : base() { }

        public static PagamentoPix Create(Dinheiro valor, string chavePix, int minutosParaVencimento = 30)
        {
            if (valor == null || valor.Valor <= 0)
                throw new ValidacaoException(nameof(Valor), "Valor deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(chavePix))
                throw new ValidacaoException(nameof(chavePix), "Chave PIX é obrigatória.");

            if (minutosParaVencimento <= 0)
                throw new ValidacaoException(nameof(minutosParaVencimento), "Tempo de vencimento inválido.");

            return new PagamentoPix
            {
                Valor = valor,
                ChavePix = chavePix.Trim(),
                DataVencimento = DateTime.Now.AddMinutes(minutosParaVencimento)
            };
        }

        public void GerarQRCode(string qrCode, string codigoPix)
        {
            if (string.IsNullOrWhiteSpace(qrCode))
                throw new ArgumentException("QR Code inválido.");

            if (string.IsNullOrWhiteSpace(codigoPix))
                throw new ArgumentException("Código PIX inválido.");

            QRCode = qrCode;
            CodigoPix = codigoPix;
        }

        public bool EstaVencido()
        {
            return DateTime.Now > DataVencimento;
        }

        public TimeSpan TempoRestante()
        {
            var tempo = DataVencimento - DateTime.Now;
            return tempo.TotalSeconds > 0 ? tempo : TimeSpan.Zero;
        }

        public override string ObterTipoPagamento() => "PIX";
    }
