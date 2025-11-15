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

            var Carrinho = new Carrinho();

            Carrinho.Cliente = this;  // associa o cliente ao carrinho 
            //  carrinho.IdCliente = this.IdCliente; se quiser associar por ID 

            Carrinhos.Add(Carrinho);
        }

    }
}
