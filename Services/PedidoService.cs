using Microsoft.EntityFrameworkCore;
using Padoka.DTOs;
using Padoka.Infraestrutura;
using Padoka.Models;

namespace Padoka.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly PadokaContext _context;

        public PedidoService(PadokaContext context)
        {
            _context = context;
        }

        public async Task<PedidoCriadoDTO> CriarPedidoAsync(long usuarioId, CriarPedidoRequestDTO request)
        {
            var numeroPedido = await GerarNumeroPedidoAsync();

            var pedido = new Pedido
            {
                NumeroPedido = numeroPedido,
                Mesa = request.Mesa,
                Observacoes = request.Observacoes,
                Status = StatusPedido.Recebido,
                UsuarioId = usuarioId,
                CriadoEm = DateTime.UtcNow
            };

            decimal valorTotal = 0;

            foreach (var itemReq in request.Itens)
            {
                var itemCardapio = await _context.ItensCardapio
                    .Include(i => i.OpcoesAdicionais)
                    .FirstOrDefaultAsync(i => i.Id == itemReq.ItemCardapioId && i.Ativo);

                if (itemCardapio == null)
                    throw new InvalidOperationException($"Item {itemReq.ItemCardapioId} não encontrado ou indisponível.");

                decimal precoUnitario = itemCardapio.Preco;

                var itemPedido = new ItemPedido
                {
                    ItemCardapioId = itemReq.ItemCardapioId,
                    Quantidade = itemReq.Quantidade,
                    PrecoUnitario = precoUnitario,
                    Observacoes = itemReq.Observacoes
                };

                if (itemReq.OpcoesAdicionaisIds != null && itemReq.OpcoesAdicionaisIds.Any())
                {
                    foreach (var opcaoId in itemReq.OpcoesAdicionaisIds)
                    {
                        var opcao = itemCardapio.OpcoesAdicionais.FirstOrDefault(o => o.Id == opcaoId && o.Ativo);
                        if (opcao != null)
                        {
                            precoUnitario += opcao.PrecoAdicional;
                            itemPedido.OpcoesAdicionais.Add(new ItemPedidoOpcao
                            {
                                OpcaoAdicionalId = opcaoId,
                                PrecoAdicional = opcao.PrecoAdicional
                            });
                        }
                    }
                }

                itemPedido.PrecoUnitario = precoUnitario;
                itemPedido.PrecoTotal = precoUnitario * itemReq.Quantidade;
                valorTotal += itemPedido.PrecoTotal;

                pedido.Itens.Add(itemPedido);
            }

            pedido.ValorTotal = valorTotal;

            pedido.HistoricoStatus.Add(new HistoricoStatusPedido
            {
                StatusAnterior = StatusPedido.Recebido,
                StatusNovo = StatusPedido.Recebido,
                AlteradoEm = DateTime.UtcNow
            });

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return new PedidoCriadoDTO
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                ValorTotal = pedido.ValorTotal,
                Mensagem = "Pedido realizado com sucesso!"
            };
        }

        public async Task<List<PedidoResumoDTO>> ObterPedidosDoUsuarioAsync(long usuarioId)
        {
            var pedidos = await _context.Pedidos
                .Where(p => p.UsuarioId == usuarioId)
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.ItemCardapio)
                .OrderByDescending(p => p.CriadoEm)
                .ToListAsync();

            return pedidos.Select(MapToPedidoResumo).ToList();
        }

        public async Task<PedidoDetalheDTO?> ObterPedidoDetalheAsync(long pedidoId, long? usuarioId = null)
        {
            var query = _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.ItemCardapio)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.OpcoesAdicionais)
                        .ThenInclude(o => o.OpcaoAdicional)
                .Include(p => p.HistoricoStatus)
                    .ThenInclude(h => h.AlteradoPor)
                .AsQueryable();

            if (usuarioId.HasValue)
            {
                query = query.Where(p => p.Id == pedidoId && p.UsuarioId == usuarioId.Value);
            }
            else
            {
                query = query.Where(p => p.Id == pedidoId);
            }

            var pedido = await query.FirstOrDefaultAsync();

            if (pedido == null) return null;

            return new PedidoDetalheDTO
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                Mesa = pedido.Mesa,
                Status = GetStatusNome(pedido.Status),
                StatusCodigo = (int)pedido.Status,
                ValorTotal = pedido.ValorTotal,
                Observacoes = pedido.Observacoes,
                CriadoEm = pedido.CriadoEm,
                AtualizadoEm = pedido.AtualizadoEm,
                Cliente = pedido.Usuario != null ? new ClienteDTO
                {
                    Id = pedido.Usuario.Id,
                    Nome = pedido.Usuario.Nome ?? "Cliente",
                    Email = pedido.Usuario.Email ?? "",
                    Telefone = null
                } : new ClienteDTO { Id = 0, Nome = "Cliente", Email = "" },
                Itens = pedido.Itens.Select(i => new ItemPedidoDTO
                {
                    Id = i.Id,
                    Nome = i.ItemCardapio.Nome,
                    ImagemUrl = i.ItemCardapio.ImagemUrl,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    PrecoTotal = i.PrecoTotal,
                    Observacoes = i.Observacoes,
                    OpcoesAdicionais = i.OpcoesAdicionais.Select(o => new OpcaoAdicionalPedidoDTO
                    {
                        Id = o.OpcaoAdicionalId,
                        Nome = o.OpcaoAdicional.Nome,
                        Preco = o.PrecoAdicional
                    }).ToList()
                }).ToList(),
                Historico = pedido.HistoricoStatus
                    .OrderByDescending(h => h.AlteradoEm)
                    .Select(h => new HistoricoStatusDTO
                    {
                        StatusAnterior = GetStatusNome(h.StatusAnterior),
                        StatusNovo = GetStatusNome(h.StatusNovo),
                        AlteradoEm = h.AlteradoEm,
                        AlteradoPor = h.AlteradoPor?.Nome
                    }).ToList()
            };
        }

        public async Task<List<PedidoResumoDTO>> ObterTodosPedidosAsync(StatusPedido? status = null, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var query = _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.ItemCardapio)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            if (dataInicio.HasValue)
            {
                query = query.Where(p => p.CriadoEm >= dataInicio.Value);
            }

            if (dataFim.HasValue)
            {
                query = query.Where(p => p.CriadoEm <= dataFim.Value);
            }

            var pedidos = await query
                .OrderByDescending(p => p.CriadoEm)
                .ToListAsync();

            return pedidos.Select(p => new PedidoResumoDTO
            {
                Id = p.Id,
                NumeroPedido = p.NumeroPedido,
                Mesa = p.Mesa,
                Status = GetStatusNome(p.Status),
                StatusCodigo = (int)p.Status,
                ValorTotal = p.ValorTotal,
                CriadoEm = p.CriadoEm,
                TotalItens = p.Itens.Sum(i => i.Quantidade),
                NomeCliente = p.Usuario.Nome
            }).ToList();
        }

        public async Task<bool> AtualizarStatusPedidoAsync(long pedidoId, StatusPedido novoStatus, long adminId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.HistoricoStatus)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null) return false;

            var statusAnterior = pedido.Status;

            pedido.Status = novoStatus;
            pedido.AtualizadoEm = DateTime.UtcNow;

            pedido.HistoricoStatus.Add(new HistoricoStatusPedido
            {
                StatusAnterior = statusAnterior,
                StatusNovo = novoStatus,
                AlteradoEm = DateTime.UtcNow,
                AlteradoPorId = adminId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DashboardDTO> ObterDashboardAsync()
        {
            var hoje = DateTime.UtcNow.Date;
            var amanha = hoje.AddDays(1);

            var pedidosHoje = await _context.Pedidos
                .Where(p => p.CriadoEm >= hoje && p.CriadoEm < amanha)
                .ToListAsync();

            var pedidosPendentes = await _context.Pedidos
                .CountAsync(p => p.Status == StatusPedido.Recebido);

            var pedidosEmPreparo = await _context.Pedidos
                .CountAsync(p => p.Status == StatusPedido.EmPreparo);

            var pedidosProntos = await _context.Pedidos
                .CountAsync(p => p.Status == StatusPedido.Pronto);

            var pedidosEntreguesHoje = await _context.Pedidos
                .CountAsync(p => p.Status == StatusPedido.Entregue && p.CriadoEm >= hoje && p.CriadoEm < amanha);

            var totalItensCardapio = await _context.ItensCardapio.CountAsync(i => i.Ativo);

            var totalClientes = await _context.Usuarios
                .CountAsync(u => u.Tipo == TipoUsuario.Cliente);

            var ultimosPedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                .OrderByDescending(p => p.CriadoEm)
                .Take(10)
                .ToListAsync();

            var itensMaisVendidos = await _context.ItensPedido
                .Where(i => i.Pedido.CriadoEm >= hoje.AddDays(-30))
                .GroupBy(i => new { i.ItemCardapioId, i.ItemCardapio.Nome })
                .Select(g => new ItemMaisVendidoDTO
                {
                    Id = g.Key.ItemCardapioId,
                    Nome = g.Key.Nome,
                    QuantidadeVendida = g.Sum(i => i.Quantidade),
                    TotalVendido = g.Sum(i => i.PrecoTotal)
                })
                .OrderByDescending(x => x.QuantidadeVendida)
                .Take(5)
                .ToListAsync();

            return new DashboardDTO
            {
                PedidosHoje = pedidosHoje.Count,
                FaturamentoHoje = pedidosHoje.Sum(p => p.ValorTotal),
                PedidosPendentes = pedidosPendentes,
                PedidosEmPreparo = pedidosEmPreparo,
                PedidosProntos = pedidosProntos,
                PedidosEntreguesHoje = pedidosEntreguesHoje,
                TotalItensCardapio = totalItensCardapio,
                TotalClientes = totalClientes,
                UltimosPedidos = ultimosPedidos.Select(p => new PedidoResumoDTO
                {
                    Id = p.Id,
                    NumeroPedido = p.NumeroPedido,
                    Mesa = p.Mesa,
                    Status = GetStatusNome(p.Status),
                    StatusCodigo = (int)p.Status,
                    ValorTotal = p.ValorTotal,
                    CriadoEm = p.CriadoEm,
                    TotalItens = p.Itens.Sum(i => i.Quantidade),
                    NomeCliente = p.Usuario.Nome
                }).ToList(),
                ItensMaisVendidos = itensMaisVendidos
            };
        }

        private async Task<string> GerarNumeroPedidoAsync()
        {
            var hoje = DateTime.UtcNow;
            var prefixo = hoje.ToString("yyMMdd");

            var ultimoPedido = await _context.Pedidos
                .Where(p => p.NumeroPedido.StartsWith(prefixo))
                .OrderByDescending(p => p.NumeroPedido)
                .FirstOrDefaultAsync();

            int sequencia = 1;
            if (ultimoPedido != null)
            {
                var ultimaSequencia = ultimoPedido.NumeroPedido.Substring(6);
                if (int.TryParse(ultimaSequencia, out int seq))
                {
                    sequencia = seq + 1;
                }
            }

            return $"{prefixo}{sequencia:D4}";
        }

        private static string GetStatusNome(StatusPedido status)
        {
            return status switch
            {
                StatusPedido.Recebido => "Recebido",
                StatusPedido.EmPreparo => "Em Preparo",
                StatusPedido.Pronto => "Pronto",
                StatusPedido.Entregue => "Entregue",
                _ => "Desconhecido"
            };
        }

        private static PedidoResumoDTO MapToPedidoResumo(Pedido pedido)
        {
            return new PedidoResumoDTO
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                Mesa = pedido.Mesa,
                Status = GetStatusNome(pedido.Status),
                StatusCodigo = (int)pedido.Status,
                ValorTotal = pedido.ValorTotal,
                CriadoEm = pedido.CriadoEm,
                TotalItens = pedido.Itens.Sum(i => i.Quantidade),
                NomeCliente = pedido.Usuario?.Nome ?? "Cliente",
                ItensResumo = string.Join(", ", pedido.Itens.Select(i => i.ItemCardapio?.Nome ?? "Item").Take(5))
            };
        }
    }
}
