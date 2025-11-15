namespace Domain.Entities
{
	public class PagamentoCartao : Pagamento
	{
		public string NumeroCartao { get; set; }
		public string NomeTitular { get; set; }
		public string Validade { get; set; }
		public string Cvv { get; set; }
		public int Parcelas { get; set; }
	}
}
