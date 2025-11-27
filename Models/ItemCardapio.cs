using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padoka.Models
{
    public class ItemCardapio
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "VARCHAR(200)")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        [Column(TypeName = "VARCHAR(300)")]
        public string DescricaoResumida { get; set; } = string.Empty;

        [StringLength(1000)]
        [Column(TypeName = "VARCHAR(1000)")]
        public string? DescricaoCompleta { get; set; }

        [StringLength(500)]
        [Column(TypeName = "VARCHAR(500)")]
        public string? Ingredientes { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal Preco { get; set; }

        [StringLength(500)]
        [Column(TypeName = "VARCHAR(500)")]
        public string? ImagemUrl { get; set; }

        public bool Ativo { get; set; } = true;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? AtualizadoEm { get; set; }

        public long CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; } = null!;

        public ICollection<OpcaoAdicional> OpcoesAdicionais { get; set; } = new List<OpcaoAdicional>();
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}
