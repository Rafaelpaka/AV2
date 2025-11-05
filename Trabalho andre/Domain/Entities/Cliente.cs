using System.Collections.Generic;

namespace Domain.Entities
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }

        public List<Carrinho> Carrinhos { get; set; }

        public Cliente()
        {
            Carrinhos = new List<Carrinho>();
            Carrinhos.Add(new Carrinho());
        }
    }
}
