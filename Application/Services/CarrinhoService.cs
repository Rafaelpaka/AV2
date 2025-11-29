using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class CarrinhoService
    {
        private readonly ICarrinhoRepository _carrinhoRepo;
        private readonly IProdutoRepository _produtoRepo;

        public CarrinhoService(ICarrinhoRepository carrinhoRepo, IProdutoRepository produtoRepo)
        {
            _carrinhoRepo = carrinhoRepo;
            _produtoRepo = produtoRepo;
        }

        public CarrinhoResponseDTO CriarCarrinho(CarrinhoCreateDTO dto)
        {
            var carrinho = new Carrinho
            {
                IdCliente = dto.IdCliente
            };

            _carrinhoRepo.Adicionar(carrinho);

            return new CarrinhoResponseDTO
            {
                IdCarrinho = carrinho.IdCarrinho,
                IdCliente = carrinho.IdCliente,
                Total = carrinho.Total,
                Itens = new List<CarrinhoItemDTO>()
            };
        }

        public CarrinhoResponseDTO AdicionarProduto(int idCarrinho, int idProduto)
        {
            var carrinho = _carrinhoRepo.ObterPorId(idCarrinho)
                ?? throw new Exception("Carrinho não encontrado.");

            var produto = _produtoRepo.ObterPorId(idProduto)
                ?? throw new Exception("Produto não encontrado.");

            carrinho.AdicionarProduto(produto);
            _carrinhoRepo.Atualizar(carrinho);

            return MapCarrinhoToDTO(carrinho);
        }

        public CarrinhoResponseDTO RemoverProduto(int idCarrinho, int idProduto)
        {
            var carrinho = _carrinhoRepo.ObterPorId(idCarrinho)
                ?? throw new Exception("Carrinho não encontrado.");

            var produto = carrinho.Itens.FirstOrDefault(p => p.IdProduto == idProduto)
                ?? throw new Exception("Produto não encontrado no carrinho.");

            carrinho.RemoverProduto(produto);
            _carrinhoRepo.Atualizar(carrinho);

            return MapCarrinhoToDTO(carrinho);
        }

        public decimal Total(int idCarrinho)
        {
            var carrinho = _carrinhoRepo.ObterPorId(idCarrinho)
                ?? throw new Exception("Carrinho não encontrado.");

            return carrinho.Total;
        }

        
        private CarrinhoResponseDTO MapCarrinhoToDTO(Carrinho carrinho)
        {
            return new CarrinhoResponseDTO
            {
                IdCarrinho = carrinho.IdCarrinho,
                IdCliente = carrinho.IdCliente,
                Total = carrinho.Total,
                Itens = carrinho.Itens.Select(p => new CarrinhoItemDTO
                {
                    IdProduto = p.IdProduto,
                    Nome = p.Nome,
                    Preco = p.Preco
                }).ToList()
            };
        }
    }
}