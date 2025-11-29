using System;
using System.Text.RegularExpressions;

namespace AV2.Domain.ValueObjects
{
    public class Email
    {
        public string Endereco { get; private set; }

        private Email(string endereco)
        {
            Endereco = endereco;
        }

        public static Email Create(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
                throw new ArgumentException("Email não pode ser vazio.");

            if (!Regex.IsMatch(endereco, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Email inválido.");

            return new Email(endereco.ToLower().Trim());
        }

        public override bool Equals(object? obj)
        {
            if (obj is Email other)
                return Endereco == other.Endereco;
            return false;
        }

        public override int GetHashCode()
        {
            return Endereco.GetHashCode();
        }

        public override string ToString()
        {
            return Endereco;
        }

        public static bool operator ==(Email? a, Email? b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(Email? a, Email? b)
        {
            return !(a == b);
        }
    }
}