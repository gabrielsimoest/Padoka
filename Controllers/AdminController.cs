using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Padoka.DTOs;
using Padoka.Models;
using Padoka.Services;
using System.Security.Claims;

namespace Padoka.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPedidoService _pedidoService;
        private readonly IAdminCardapioService _adminCardapioService;

        public AdminController(IPedidoService pedidoService, IAdminCardapioService adminCardapioService)
        {
            _pedidoService = pedidoService;
            _adminCardapioService = adminCardapioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Pedidos()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cardapio()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Categorias()
        {
            return View();
        }

        [HttpGet]
        [Route("api/admin/dashboard")]
        [Authorize]
        public async Task<IActionResult> ObterDashboard()
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var dashboard = await _pedidoService.ObterDashboardAsync();
            return Ok(new { sucesso = true, dados = dashboard });
        }

        [HttpGet]
        [Route("api/admin/pedidos")]
        [Authorize]
        public async Task<IActionResult> ObterPedidos([FromQuery] int? status, [FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            StatusPedido? statusFiltro = status.HasValue ? (StatusPedido)status.Value : null;
            var pedidos = await _pedidoService.ObterTodosPedidosAsync(statusFiltro, dataInicio, dataFim);
            return Ok(new { sucesso = true, dados = pedidos });
        }

        [HttpGet]
        [Route("api/admin/pedido/{id}")]
        [Authorize]
        public async Task<IActionResult> ObterPedido(long id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var pedido = await _pedidoService.ObterPedidoDetalheAsync(id);
            if (pedido == null)
            {
                return NotFound(new { sucesso = false, mensagem = "Pedido não encontrado." });
            }
            return Ok(new { sucesso = true, dados = pedido });
        }

        [HttpPut]
        [Route("api/admin/pedidos/{id}/status")]
        [Authorize]
        public async Task<IActionResult> AtualizarStatusPedido(long id, [FromBody] AtualizarStatusPedidoDTO request)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var adminId = GetUserId();
            if (adminId == null)
            {
                return Unauthorized();
            }

            var resultado = await _pedidoService.AtualizarStatusPedidoAsync(id, (StatusPedido)request.Status, adminId.Value);
            if (!resultado)
            {
                return NotFound(new { sucesso = false, mensagem = "Pedido não encontrado." });
            }

            return Ok(new { sucesso = true, mensagem = "Status atualizado com sucesso." });
        }

        [HttpGet]
        [Route("api/admin/categorias")]
        [Authorize]
        public async Task<IActionResult> ObterCategorias()
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var categorias = await _adminCardapioService.ObterCategoriasAsync();
            return Ok(new { sucesso = true, dados = categorias });
        }

        [HttpGet]
        [Route("api/admin/categorias/{id}")]
        [Authorize]
        public async Task<IActionResult> ObterCategoria(long id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var categoria = await _adminCardapioService.ObterCategoriaAsync(id);
            if (categoria == null)
            {
                return NotFound(new { sucesso = false, mensagem = "Categoria não encontrada." });
            }
            return Ok(new { sucesso = true, dados = categoria });
        }

        [HttpPost]
        [Route("api/admin/categorias")]
        [Authorize]
        public async Task<IActionResult> CriarCategoria([FromBody] CriarCategoriaDTO request)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var categoria = await _adminCardapioService.CriarCategoriaAsync(request);
            return Ok(new { sucesso = true, dados = categoria, mensagem = "Categoria criada com sucesso." });
        }

        [HttpPut]
        [Route("api/admin/categorias/{id}")]
        [Authorize]
        public async Task<IActionResult> AtualizarCategoria(long id, [FromBody] AtualizarCategoriaDTO request)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var categoria = await _adminCardapioService.AtualizarCategoriaAsync(id, request);
            if (categoria == null)
            {
                return NotFound(new { sucesso = false, mensagem = "Categoria não encontrada." });
            }
            return Ok(new { sucesso = true, dados = categoria, mensagem = "Categoria atualizada com sucesso." });
        }

        [HttpDelete]
        [Route("api/admin/categorias/{id}")]
        [Authorize]
        public async Task<IActionResult> ExcluirCategoria(long id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            try
            {
                var resultado = await _adminCardapioService.ExcluirCategoriaAsync(id);
                if (!resultado)
                {
                    return NotFound(new { sucesso = false, mensagem = "Categoria não encontrada." });
                }
                return Ok(new { sucesso = true, mensagem = "Categoria excluída com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { sucesso = false, mensagem = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/admin/itens")]
        [Authorize]
        public async Task<IActionResult> ObterItens([FromQuery] long? categoriaId)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var itens = await _adminCardapioService.ObterItensAsync(categoriaId);
            return Ok(new { sucesso = true, dados = itens });
        }

        [HttpGet]
        [Route("api/admin/itens/{id}")]
        [Authorize]
        public async Task<IActionResult> ObterItem(long id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var item = await _adminCardapioService.ObterItemAsync(id);
            if (item == null)
            {
                return NotFound(new { sucesso = false, mensagem = "Item não encontrado." });
            }
            return Ok(new { sucesso = true, dados = item });
        }

        [HttpPost]
        [Route("api/admin/itens")]
        [Authorize]
        public async Task<IActionResult> CriarItem([FromBody] CriarItemCardapioDTO request)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            try
            {
                var item = await _adminCardapioService.CriarItemAsync(request);
                return Ok(new { sucesso = true, dados = item, mensagem = "Item criado com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { sucesso = false, mensagem = ex.Message });
            }
        }

        [HttpPut]
        [Route("api/admin/itens/{id}")]
        [Authorize]
        public async Task<IActionResult> AtualizarItem(long id, [FromBody] AtualizarItemCardapioDTO request)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            try
            {
                var item = await _adminCardapioService.AtualizarItemAsync(id, request);
                if (item == null)
                {
                    return NotFound(new { sucesso = false, mensagem = "Item não encontrado." });
                }
                return Ok(new { sucesso = true, dados = item, mensagem = "Item atualizado com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { sucesso = false, mensagem = ex.Message });
            }
        }

        [HttpDelete]
        [Route("api/admin/itens/{id}")]
        [Authorize]
        public async Task<IActionResult> ExcluirItem(long id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var resultado = await _adminCardapioService.ExcluirItemAsync(id);
            if (!resultado)
            {
                return NotFound(new { sucesso = false, mensagem = "Item não encontrado." });
            }
            return Ok(new { sucesso = true, mensagem = "Item excluído com sucesso." });
        }

        [HttpPut]
        [Route("api/admin/itens/{id}/disponibilidade")]
        [Authorize]
        public async Task<IActionResult> AlterarDisponibilidade(long id, [FromBody] AlterarDisponibilidadeDTO request)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var resultado = await _adminCardapioService.AlterarDisponibilidadeAsync(id, request.Disponivel);
            if (!resultado)
            {
                return NotFound(new { sucesso = false, mensagem = "Item não encontrado." });
            }
            return Ok(new { sucesso = true, mensagem = "Disponibilidade alterada com sucesso." });
        }

        [HttpGet]
        [Route("api/admin/item/{itemId}/opcoes")]
        [Authorize]
        public async Task<IActionResult> ObterOpcoes(long itemId)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var opcoes = await _adminCardapioService.ObterOpcoesDoItemAsync(itemId);
            return Ok(new { sucesso = true, dados = opcoes });
        }

        [HttpPost]
        [Route("api/admin/opcao")]
        [Authorize]
        public async Task<IActionResult> CriarOpcao([FromBody] CriarOpcaoAdicionalDTO request)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            try
            {
                var opcao = await _adminCardapioService.CriarOpcaoAsync(request);
                return Ok(new { sucesso = true, dados = opcao, mensagem = "Opção criada com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { sucesso = false, mensagem = ex.Message });
            }
        }

        [HttpPut]
        [Route("api/admin/opcao/{id}")]
        [Authorize]
        public async Task<IActionResult> AtualizarOpcao(long id, [FromBody] AtualizarOpcaoAdicionalDTO request)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var opcao = await _adminCardapioService.AtualizarOpcaoAsync(id, request);
            if (opcao == null)
            {
                return NotFound(new { sucesso = false, mensagem = "Opção não encontrada." });
            }
            return Ok(new { sucesso = true, dados = opcao, mensagem = "Opção atualizada com sucesso." });
        }

        [HttpDelete]
        [Route("api/admin/opcao/{id}")]
        [Authorize]
        public async Task<IActionResult> ExcluirOpcao(long id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var resultado = await _adminCardapioService.ExcluirOpcaoAsync(id);
            if (!resultado)
            {
                return NotFound(new { sucesso = false, mensagem = "Opção não encontrada." });
            }
            return Ok(new { sucesso = true, mensagem = "Opção excluída com sucesso." });
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Administrador") || 
                   User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Administrador");
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
