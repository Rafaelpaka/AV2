using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrinhoController : ControllerBase
    {
        // Lista estática de itens
        private static List<CarrinhoItem> carrinho = new List<CarrinhoItem>();

        [HttpGet]
        public ActionResult<IEnumerable<CarrinhoItem>> GetCarrinho()
        {
            return Ok(carrinho);
        }

        [HttpPost]
        public ActionResult<CarrinhoItem> AdicionarAoCarrinho([FromBody] AdicionarCarrinhoRequest request)
        {
            var itemExistente = carrinho.FirstOrDefault(c => c.ProdutoId == request.ProdutoId);
            
            if (itemExistente != null)
            {
                itemExistente.Quantidade += request.Quantidade;
                return Ok(itemExistente);
            }
            
            var novoItem = new CarrinhoItem
            {
                Id = carrinho.Count + 1,
                ProdutoId = request.ProdutoId,
                Quantidade = request.Quantidade
            };
            
            carrinho.Add(novoItem);
            return CreatedAtAction(nameof(GetCarrinho), novoItem);
        }

        [HttpDelete("{id}")]
        public ActionResult RemoverDoCarrinho(int id)
        {
            var item = carrinho.FirstOrDefault(c => c.Id == id);
            if (item == null)
            {
                return NotFound(new { message = "Item não encontrado no carrinho" });
            }
            
            carrinho.Remove(item);
            return Ok(new { message = "Item removido do carrinho", deleted = true });
        }

        [HttpDelete]
        public ActionResult LimparCarrinho()
        {
            carrinho.Clear();
            return Ok(new { message = "Carrinho limpo com sucesso" });
        }
    }

    public class CarrinhoItem
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }

    public class AdicionarCarrinhoRequest
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}