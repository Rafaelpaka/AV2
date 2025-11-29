using System.Collections.Generic;
using AV2.Application.DTOs.CarrinhoDTOs;
namespace AV2.Application.DTOs.CarrinhoDTOs
{
    public class CarrinhoResponseDTO
    {
        public int IdCarrinho { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public required List<CarrinhoItemDTO> Itens { get; set; }
    }
}