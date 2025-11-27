using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padoka.Models
{
    public class OpcaoAdicional
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR(100)")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(300)]
        [Column(TypeName = "VARCHAR(300)")]
        public string? Descricao { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal PrecoAdicional { get; set; }

        public bool Ativo { get; set; } = true;

        public long ItemCardapioId { get; set; }
        [ForeignKey("ItemCardapioId")]
        public ItemCardapio ItemCardapio { get; set; } = null!;

        public ICollection<ItemPedidoOpcao> ItensPedidoOpcao { get; set; } = new List<ItemPedidoOpcao>();
    }
}
