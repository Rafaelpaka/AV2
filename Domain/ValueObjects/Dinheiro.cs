using System;

namespace AV2.Domain.ValueObjects
{
    public class Dinheiro
    {
        public decimal Valor { get; private set; }
        public string Moeda { get; private set; }

        private Dinheiro(decimal valor, string moeda)
        {
            Valor = valor;
            Moeda = moeda;
        }

        public static Dinheiro Create(decimal valor, string moeda = "BRL")
        {
            if (valor < 0)
                throw new ArgumentException("Valor não pode ser negativo.");

            if (string.IsNullOrWhiteSpace(moeda))
                throw new ArgumentException("Moeda deve ser especificada.");

            return new Dinheiro(Math.Round(valor, 2), moeda.ToUpper());
        }

        public static Dinheiro Zero(string moeda = "BRL")
        {
            return new Dinheiro(0, moeda);
        }

        // Operações matemáticas
        public Dinheiro Adicionar(Dinheiro outro)
        {
            ValidarMesmaMoeda(outro);
            return new Dinheiro(Valor + outro.Valor, Moeda);
        }

        public Dinheiro Subtrair(Dinheiro outro)
        {
            ValidarMesmaMoeda(outro);
            if (Valor < outro.Valor)
                throw new InvalidOperationException("Resultado não pode ser negativo.");
            return new Dinheiro(Valor - outro.Valor, Moeda);
        }

        public Dinheiro Multiplicar(decimal fator)
        {
            if (fator < 0)
                throw new ArgumentException("Fator não pode ser negativo.");
            return new Dinheiro(Valor * fator, Moeda);
        }

        public Dinheiro Dividir(decimal divisor)
        {
            if (divisor <= 0)
                throw new ArgumentException("Divisor deve ser maior que zero.");
            return new Dinheiro(Valor / divisor, Moeda);
        }

        private void ValidarMesmaMoeda(Dinheiro outro)
        {
            if (Moeda != outro.Moeda)
                throw new InvalidOperationException($"Não é possível operar com moedas diferentes: {Moeda} e {outro.Moeda}");
        }

        public string Formatar()
        {
            return Moeda switch
            {
                "BRL" => $"R$ {Valor:N2}",
                "USD" => $"$ {Valor:N2}",
                "EUR" => $"€ {Valor:N2}",
                _ => $"{Moeda} {Valor:N2}"
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is Dinheiro other)
                return Valor == other.Valor && Moeda == other.Moeda;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Valor, Moeda);
        }

        public override string ToString()
        {
            return Formatar();
        }

        // Operadores
        public static Dinheiro operator +(Dinheiro a, Dinheiro b) => a.Adicionar(b);
        public static Dinheiro operator -(Dinheiro a, Dinheiro b) => a.Subtrair(b);
        public static Dinheiro operator *(Dinheiro a, decimal b) => a.Multiplicar(b);
        public static Dinheiro operator /(Dinheiro a, decimal b) => a.Dividir(b);

        public static bool operator ==(Dinheiro a, Dinheiro b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(Dinheiro a, Dinheiro b) => !(a == b);
        public static bool operator >(Dinheiro a, Dinheiro b)
        {
            a.ValidarMesmaMoeda(b);
            return a.Valor > b.Valor;
        }
        public static bool operator <(Dinheiro a, Dinheiro b)
        {
            a.ValidarMesmaMoeda(b);
            return a.Valor < b.Valor;
        }
        public static bool operator >=(Dinheiro a, Dinheiro b) => a > b || a == b;
        public static bool operator <=(Dinheiro a, Dinheiro b) => a < b || a == b;
    }
}