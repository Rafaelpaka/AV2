using System.Collections.Generic;

namespace Application.DTOs.CarrinhoDTOs
{
    public class CarrinhoResponseDTO
    {
        public int IdCarrinho { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }

        public List<CarrinhoItemDTO> Itens { get; set; }
    }
}