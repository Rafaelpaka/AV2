namespace Domain.Entities
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public Cliente cliente { get; private set; }
        public string Status { get; set; }
        public decimal Total { get; set; }

        // Você pode adicionar listas de produtos futuramente, se desejar.
    }
}
