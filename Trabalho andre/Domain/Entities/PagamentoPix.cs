using System;
using Domain.Entities;

namespace Domain.Entities
{
	public class PagamentoPix : Pagamento
	{
		public string ChavePix { get; set; }
		public DateTime DataPagamento { get; set; }
	}
}
