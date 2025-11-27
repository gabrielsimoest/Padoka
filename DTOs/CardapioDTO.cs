namespace Padoka.DTOs
{
    public class CategoriaDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
        public List<ItemCardapioResumoDTO> Itens { get; set; } = new();
    }

    public class ItemCardapioResumoDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string DescricaoResumida { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public long CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
    }

    public class ItemCardapioDetalheDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string? DescricaoCompleta { get; set; }
        public string? Ingredientes { get; set; }
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public long CategoriaId { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public List<OpcaoAdicionalDTO> OpcoesAdicionais { get; set; } = new();
    }

    public class OpcaoAdicionalDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
