using Padoka.DTOs;
using Padoka.Models;

namespace Padoka.Services
{
    public interface IPedidoService
    {
        Task<PedidoCriadoDTO> CriarPedidoAsync(long usuarioId, CriarPedidoRequestDTO request);
        Task<List<PedidoResumoDTO>> ObterPedidosDoUsuarioAsync(long usuarioId);
        Task<PedidoDetalheDTO?> ObterPedidoDetalheAsync(long pedidoId, long? usuarioId = null);
        Task<List<PedidoResumoDTO>> ObterTodosPedidosAsync(StatusPedido? status = null, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<bool> AtualizarStatusPedidoAsync(long pedidoId, StatusPedido novoStatus, long adminId);
        Task<DashboardDTO> ObterDashboardAsync();
    }
}
