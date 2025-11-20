using System;

namespace Domain.ValueObjects
{
    public class Endereco
    {
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string CEP { get; private set; }
        public string Pais { get; private set; }

        private Endereco(string logradouro, string numero, string complemento, 
                        string bairro, string cidade, string estado, string cep, string pais)
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            CEP = cep;
            Pais = pais;
        }

        public static Endereco Create(string logradouro, string numero, string complemento,
                                      string bairro, string cidade, string estado, string cep, 
                                      string pais = "Brasil")
        {
            if (string.IsNullOrWhiteSpace(logradouro))
                throw new ArgumentException("Logradouro é obrigatório.");

            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("Número é obrigatório.");

            if (string.IsNullOrWhiteSpace(bairro))
                throw new ArgumentException("Bairro é obrigatório.");

            if (string.IsNullOrWhiteSpace(cidade))
                throw new ArgumentException("Cidade é obrigatória.");

            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("Estado é obrigatório.");

            if (string.IsNullOrWhiteSpace(cep))
                throw new ArgumentException("CEP é obrigatório.");

            // Remove caracteres não numéricos do CEP
            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());
            if (cepLimpo.Length != 8)
                throw new ArgumentException("CEP deve ter 8 dígitos.");

            return new Endereco(
                logradouro.Trim(),
                numero.Trim(),
                complemento?.Trim() ?? string.Empty,
                bairro.Trim(),
                cidade.Trim(),
                estado.Trim(),
                cepLimpo,
                pais?.Trim() ?? "Brasil"
            );
        }

        public string EnderecoCompleto()
        {
            var complementoTexto = !string.IsNullOrWhiteSpace(Complemento) 
                ? $", {Complemento}" 
                : string.Empty;

            return $"{Logradouro}, {Numero}{complementoTexto} - {Bairro}, {Cidade}/{Estado} - CEP: {CEPFormatado()}";
        }

        public string CEPFormatado()
        {
            return $"{CEP.Substring(0, 5)}-{CEP.Substring(5, 3)}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Endereco other)
            {
                return Logradouro == other.Logradouro &&
                       Numero == other.Numero &&
                       Complemento == other.Complemento &&
                       Bairro == other.Bairro &&
                       Cidade == other.Cidade &&
                       Estado == other.Estado &&
                       CEP == other.CEP &&
                       Pais == other.Pais;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Logradouro, Numero, Bairro, Cidade, Estado, CEP);
        }

        public override string ToString()
        {
            return EnderecoCompleto();
        }

        public static bool operator ==(Endereco a, Endereco b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(Endereco a, Endereco b)
        {
            return !(a == b);
        }
    }
}