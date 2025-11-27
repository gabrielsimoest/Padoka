using Microsoft.AspNetCore.Mvc;
using Padoka.Services;

namespace Padoka.Controllers
{
    public class CardapioController : Controller
    {
        private readonly ICardapioService _cardapioService;

        public CardapioController(ICardapioService cardapioService)
        {
            _cardapioService = cardapioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Item(long id)
        {
            ViewData["ItemId"] = id;
            return View();
        }

        [HttpGet]
        [Route("api/cardapio")]
        public async Task<IActionResult> ObterCardapio()
        {
            var cardapio = await _cardapioService.ObterCardapioCompletoAsync();
            return Ok(new { sucesso = true, dados = cardapio });
        }

        [HttpGet]
        [Route("api/cardapio/categorias")]
        public async Task<IActionResult> ObterCategorias()
        {
            var categorias = await _cardapioService.ObterCategoriasAsync();
            return Ok(new { sucesso = true, dados = categorias });
        }

        [HttpGet]
        [Route("api/cardapio/categoria/{categoriaId}")]
        public async Task<IActionResult> ObterItensPorCategoria(long categoriaId)
        {
            var categoria = await _cardapioService.ObterCategoriaComItensAsync(categoriaId);
            return Ok(new { sucesso = true, dados = categoria });
        }

        [HttpGet]
        [Route("api/cardapio/item/{itemId}")]
        public async Task<IActionResult> ObterItemDetalhe(long itemId)
        {
            var item = await _cardapioService.ObterItemDetalheAsync(itemId);
            if (item == null)
            {
                return NotFound(new { sucesso = false, mensagem = "Item não encontrado" });
            }
            return Ok(new { sucesso = true, dados = item });
        }

        [HttpGet]
        [Route("api/cardapio/buscar")]
        public async Task<IActionResult> BuscarItens([FromQuery] string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
            {
                return BadRequest(new { sucesso = false, mensagem = "Termo de busca é obrigatório" });
            }

            var itens = await _cardapioService.BuscarItensAsync(termo);
            return Ok(new { sucesso = true, dados = itens });
        }
    }
}
