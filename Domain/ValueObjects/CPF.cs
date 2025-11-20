using System;
using System.Linq;

namespace Domain.ValueObjects
{
    public class CPF
    {
        public string Numero { get; private set; }

        private CPF(string numero)
        {
            Numero = numero;
        }

        public static CPF Create(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("CPF não pode ser vazio.");

            // Remove pontos, traços e espaços
            var cpfLimpo = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpfLimpo.Length != 11)
                throw new ArgumentException("CPF deve ter 11 dígitos.");

            if (!ValidarCPF(cpfLimpo))
                throw new ArgumentException("CPF inválido.");

            return new CPF(cpfLimpo);
        }

        private static bool ValidarCPF(string cpf)
        {
            // CPFs com todos dígitos iguais são inválidos
            if (cpf.Distinct().Count() == 1)
                return false;

            // Validação do primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digito1)
                return false;

            // Validação do segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpf[10].ToString()) == digito2;
        }

        public string NumeroFormatado()
        {
            return $"{Numero.Substring(0, 3)}.{Numero.Substring(3, 3)}.{Numero.Substring(6, 3)}-{Numero.Substring(9, 2)}";
        }

        public override bool Equals(object obj)
        {
            if (obj is CPF other)
                return Numero == other.Numero;
            return false;
        }

        public override int GetHashCode()
        {
            return Numero.GetHashCode();
        }

        public override string ToString()
        {
            return NumeroFormatado();
        }

        public static bool operator ==(CPF a, CPF b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(CPF a, CPF b)
        {
            return !(a == b);
        }
    }
}