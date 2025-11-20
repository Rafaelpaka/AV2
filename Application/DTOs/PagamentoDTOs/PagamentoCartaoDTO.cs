using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.PagamentoDTOs
{
    public class PagamentoCartaoDTO
    {
        public decimal Valor { get; set; }

        [Required]
        public string NumeroCartao { get; set; }

        [Required]
        public string NomeTitular { get; set; }

        [Required]
        public string Validade { get; set; }

        [Required]
        [MinLength(3)]
        public string Cvv { get; set; }
        public int Parcelas { get; set; }
    }
}