using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Padoka.DTOs;
using Padoka.Services;
using System.Security.Claims;

namespace Padoka.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Confirmacao(long id)
        {
            ViewData["PedidoId"] = id;
            return View();
        }
        
        [HttpGet]
        public IActionResult MeusPedidos()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Detalhe(long id)
        {
            ViewData["PedidoId"] = id;
            return View();
        }

        [HttpPost]
        [Route("api/pedido")]
        [Authorize]
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoRequestDTO request)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    return Unauthorized(new { sucesso = false, mensagem = "Usuário não autenticado." });
                }

                var resultado = await _pedidoService.CriarPedidoAsync(userId.Value, request);
                return Ok(new { sucesso = true, dados = resultado });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { sucesso = false, mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { sucesso = false, mensagem = "Erro ao criar pedido: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("api/pedido/meus-pedidos")]
        [Authorize]
        public async Task<IActionResult> ObterMeusPedidos([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { sucesso = false, mensagem = "Usuário não autenticado." });
            }

            var pedidos = await _pedidoService.ObterPedidosDoUsuarioAsync(userId.Value);
            
            // Paginação simples
            var paginados = pedidos.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();
            return Ok(new { sucesso = true, dados = paginados });
        }

        [HttpGet]
        [Route("api/pedido/{id}")]
        [Authorize]
        public async Task<IActionResult> ObterPedido(long id)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { sucesso = false, mensagem = "Usuário não autenticado." });
            }

            var isAdmin = User.Claims.Any(c => c.Type == "tipo" && c.Value == "Administrador");
            var pedido = await _pedidoService.ObterPedidoDetalheAsync(id, isAdmin ? null : userId);

            if (pedido == null)
            {
                return NotFound(new { sucesso = false, mensagem = "Pedido não encontrado." });
            }

            return Ok(new { sucesso = true, dados = pedido });
        }

        private long? GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                return userId;
            }
            return null;
        }
    }
}
