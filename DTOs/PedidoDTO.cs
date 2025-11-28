namespace Padoka.DTOs
{
    public class CriarPedidoRequestDTO
    {
        public string? Mesa { get; set; }
        public string? Observacoes { get; set; }
        public List<ItemPedidoRequestDTO> Itens { get; set; } = new();
    }

    public class ItemPedidoRequestDTO
    {
        public long ItemCardapioId { get; set; }
        public int Quantidade { get; set; }
        public string? Observacoes { get; set; }
        public List<long>? OpcoesAdicionaisIds { get; set; }
    }

    public class AtualizarStatusPedidoDTO
    {
        public int Status { get; set; }
    }

    public class PedidoResumoDTO
    {
        public long Id { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public string? Mesa { get; set; }
        public string? NumeroMesa { get { return Mesa; } }
        public string Status { get; set; } = string.Empty;
        public int StatusCodigo { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime CriadoEm { get; set; }
        public int TotalItens { get; set; }
        public int QuantidadeItens { get { return TotalItens; } }
        public string NomeCliente { get; set; } = string.Empty;
        public string? ItensResumo { get; set; }
    }

    public class PedidoDetalheDTO
    {
        public long Id { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public string? Mesa { get; set; }
        public string Status { get; set; } = string.Empty;
        public int StatusCodigo { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
        public ClienteDTO Cliente { get; set; } = null!;
        public List<ItemPedidoDTO> Itens { get; set; } = new();
        public List<HistoricoStatusDTO> Historico { get; set; } = new();
    }

    public class ClienteDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
    }

    public class ItemPedidoDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? ImagemUrl { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal { get; set; }
        public string? Observacoes { get; set; }
        public List<OpcaoAdicionalPedidoDTO> OpcoesAdicionais { get; set; } = new();
    }

    public class OpcaoAdicionalPedidoDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }

    public class HistoricoStatusDTO
    {
        public string StatusAnterior { get; set; } = string.Empty;
        public string StatusNovo { get; set; } = string.Empty;
        public DateTime AlteradoEm { get; set; }
        public string? AlteradoPor { get; set; }
    }

    public class PedidoCriadoDTO
    {
        public long Id { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string Mensagem { get; set; } = string.Empty;
    }
}
