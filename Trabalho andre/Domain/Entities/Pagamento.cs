namespace Domain.Entities
{
    public class Pagamento
    {
        public int IdPagamento { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
    }
}
