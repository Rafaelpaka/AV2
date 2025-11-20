using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.PagamentoDTOs
{
    public class PagamentoPixDTO
    {
        public decimal Valor { get; set; }

        [Required]
        public string ChavePix { get; set; }
    }
}