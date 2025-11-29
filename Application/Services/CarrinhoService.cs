using System;
using System.Collections.Generic;
using System.Linq;
using AV2.Domain.Entities;
using AV2.Domain.Interfaces;
using AV2.Application.DTOs.CarrinhoDTOs;

namespace AV2.Application.Services
{
    public class CarrinhoService
    {
        private readonly ICarrinhoRepository _carrinhoRepo;
        private readonly IProdutoRepository _produtoRepo;
        private readonly IClienteRepository _clienteRepo;

        public CarrinhoService(ICarrinhoRepository carrinhoRepo, IProdutoRepository produtoRepo, IClienteRepository clienteRepo)
        {
            _carrinhoRepo = carrinhoRepo;
            _produtoRepo = produtoRepo;
            _clienteRepo = clienteRepo;
        }

        public CarrinhoResponseDTO CriarCarrinho(CarrinhoCreateDTO dto)
        {
            var cliente = _clienteRepo.ObterPorId(dto.IdCliente)
                ?? throw new Exception("Cliente não encontrado.");

            // Usar o método factory estático
            var carrinho = Carrinho.Create(dto.IdCliente, cliente);

            _carrinhoRepo.Adicionar(carrinho);

            return new CarrinhoResponseDTO
            {
                IdCarrinho = carrinho.IdCarrinho,
                IdCliente = carrinho.IdCliente,
                Total = carrinho.CalcularTotal().Valor,
                Itens = new List<CarrinhoItemDTO>()
            };
        }

        public CarrinhoResponseDTO AdicionarProduto(int idCarrinho, int idProduto, int quantidade = 1)
        {
            var carrinho = _carrinhoRepo.ObterPorId(idCarrinho)
                ?? throw new Exception("Carrinho não encontrado.");

            var produto = _produtoRepo.ObterPorId(idProduto)
                ?? throw new Exception("Produto não encontrado.");

            carrinho.AdicionarProduto(produto, quantidade);
            _carrinhoRepo.Atualizar(carrinho);

            return MapCarrinhoToDTO(carrinho);
        }

        public CarrinhoResponseDTO RemoverProduto(int idCarrinho, int idProduto)
        {
            var carrinho = _carrinhoRepo.ObterPorId(idCarrinho)
                ?? throw new Exception("Carrinho não encontrado.");

            carrinho.RemoverProduto(idProduto);
            _carrinhoRepo.Atualizar(carrinho);

            return MapCarrinhoToDTO(carrinho);
        }

        public decimal Total(int idCarrinho)
        {
            var carrinho = _carrinhoRepo.ObterPorId(idCarrinho)
                ?? throw new Exception("Carrinho não encontrado.");

            return carrinho.CalcularTotal().Valor;
        }

        private CarrinhoResponseDTO MapCarrinhoToDTO(Carrinho carrinho)
        {
            return new CarrinhoResponseDTO
            {
                IdCarrinho = carrinho.IdCarrinho,
                IdCliente = carrinho.IdCliente,
                Total = carrinho.CalcularTotal().Valor,
                Itens = carrinho.Itens.Select(item => new CarrinhoItemDTO
                {
                    IdProduto = item.IdProduto,
                    Nome = item.Produto?.Nome ?? "Produto não disponível",
                    Preco = item.PrecoUnitarioDecimal
                }).ToList()
            };
        }
    }
}