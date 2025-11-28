namespace Padoka.DTOs
{
    public class CriarCategoriaDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public class AtualizarCategoriaDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
    }

    public class CriarItemCardapioDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string DescricaoResumida { get; set; } = string.Empty;
        public string? DescricaoCompleta { get; set; }
        public string? Ingredientes { get; set; }
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public long CategoriaId { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public class AtualizarItemCardapioDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string DescricaoResumida { get; set; } = string.Empty;
        public string? DescricaoCompleta { get; set; }
        public string? Ingredientes { get; set; }
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public long CategoriaId { get; set; }
        public bool Ativo { get; set; }
    }

    public class CriarOpcaoAdicionalDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal PrecoAdicional { get; set; }
        public long ItemCardapioId { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public class AtualizarOpcaoAdicionalDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal PrecoAdicional { get; set; }
        public bool Ativo { get; set; }
    }

    public class AlterarDisponibilidadeDTO
    {
        public bool Disponivel { get; set; }
    }

    public class CategoriaAdminDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
        public int TotalItens { get; set; }
        public int QuantidadeItens { get { return TotalItens; } }
        public DateTime CriadoEm { get; set; }
    }

    public class ItemCardapioAdminDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string DescricaoResumida { get; set; } = string.Empty;
        public string? Descricao { get { return DescricaoResumida; } }
        public string? DescricaoCompleta { get; set; }
        public string? Ingredientes { get; set; }
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public long CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
        public string? NomeCategoria { get { return CategoriaNome; } }
        public bool Ativo { get; set; }
        public bool Disponivel { get { return Ativo; } }
        public bool Destaque { get; set; }
        public int TotalOpcoes { get; set; }
        public DateTime CriadoEm { get; set; }
    }

    public class OpcaoAdicionalAdminDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal PrecoAdicional { get; set; }
        public long ItemCardapioId { get; set; }
        public bool Ativo { get; set; }
    }

    public class DashboardDTO
    {
        public int PedidosHoje { get; set; }
        public decimal FaturamentoHoje { get; set; }
        public int PedidosPendentes { get; set; }
        public int PedidosEmPreparo { get; set; }
        public int PedidosProntos { get; set; }
        public int PedidosEntreguesHoje { get; set; }
        public int TotalItensCardapio { get; set; }
        public int TotalClientes { get; set; }
        public List<PedidoResumoDTO> UltimosPedidos { get; set; } = new();
        public List<ItemMaisVendidoDTO> ItensMaisVendidos { get; set; } = new();
    }

    public class ItemMaisVendidoDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int QuantidadeVendida { get; set; }
        public decimal TotalVendido { get; set; }
    }
}
