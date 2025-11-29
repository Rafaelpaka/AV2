using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        // Lista estática de produtos
        private static List<Produto> produtos = new List<Produto>
        {
            new Produto 
            { 
                Id = 1, 
                Name = "Mouse Gamer RGB", 
                Price = 149.99m, 
                Description = "Mouse gamer com iluminação RGB, 12000 DPI, 7 botões programáveis",
                Category = "mouse",
                Image = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=400&h=300&fit=crop&crop=center",
                Rating = 4.5,
                Reviews = 128
            },
            new Produto 
            { 
                Id = 2, 
                Name = "Teclado Mecânico", 
                Price = 299.99m,
                Description = "Teclado mecânico com switches blue, RGB por tecla, layout ABNT2",
                Category = "teclado",
                Image = "https://images.unsplash.com/photo-1541140532154-b024d705b90a?w=400&h=300&fit=crop&crop=center",
                Rating = 4.7,
                Reviews = 203
            },
            // new Produto 
            // { 
            //     Id = 3, 
            //     Name = "Fone Headset Gamer", 
            //     Price = 199.99m,
            //     Description = "Headset gamer 7.1 surround, microfone removível, almofadas confortáveis",
            //     Category = "fone",
            //     Image = "https://images.unsplash.com/photo-1484704849700-f032a568e944?w=400&h=300&fit=crop&crop=center",
            //     Rating = 4.4,
            //     Reviews = 97
            // }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetProdutos()
        {
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetProduto(int id)
        {
            var produto = produtos.Find(p => p.Id == id);
            if (produto == null)
            {
                return NotFound(new { message = "Produto não encontrado" });
            }
            return Ok(produto);
        }

        [HttpPost]
        public ActionResult<Produto> CreateProduto([FromBody] Produto produto)
        {
            produto.Id = produtos.Count + 1;
            produtos.Add(produto);
            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }
    }

    // Classe Produto (pode ser movida para Domain/Entities depois)
    public class Produto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
        public double Rating { get; set; }
        public int Reviews { get; set; }
    }
}