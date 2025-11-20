using system;
using system.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public class Email
    {
        public string Endereco { get; private set; }

        private Email(string endereco)
        {
            Endereco = endereco;
        }

        public static Email Criar(string endereco)
        {
            if (string.IsNullOrEmpty(endereco))
                throw new Exception("Email não pode ser vazio");

            if (!Regex.IsMatch(endereco, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                throw new Exception("Email inválido");


            return new Email(endereço.ToLower().Trim());
        }

        public override bool Equals(object obj)
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


        public static bool operator ==(Email a, Email b)
        {
            if (referenceEquals(a, null) && referenceEquals(b, null))
                return true;
            if (referenceEquals(a, null) || referenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(Email a, Email b)
        {
            return !(a == b);
        }
    }

}