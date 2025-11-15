using Domain.Entities;
using Domain.Interfaces;

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

        public Carrinho CriarCarrinho(int idCliente)
        {
            var carrinho = new Carrinho { IdCliente = idCliente };
            _carrinhoRepo.Adicionar(carrinho);
            return carrinho;
        }

        public void AdicionarProduto(int idCarrinho, int idProduto)
        {
            var carrinho = _carrinhoRepo.ObterPorId(idCarrinho);
            var produto = _produtoRepo.ObterPorId(idProduto);

            carrinho.AdicionarProduto(produto);

            _carrinhoRepo.Atualizar(carrinho);
        }

        public void RemoverProduto(int idCarrinho, int idProduto)
        {
            var carrinho = _carrinhoRepo.ObterPorId(idCarrinho);
            var produto = _produtoRepo.ObterPorId(idProduto);

            carrinho.RemoverProduto(produto);

            _carrinhoRepo.Atualizar(carrinho);
        }

        public decimal Total(int idCarrinho)
        {
            var carrinho = _carrinhoRepo.ObterPorId(idCarrinho);
            return carrinho.Total;
        }
    }
}
