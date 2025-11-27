using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padoka.Models
{
    public class Categoria
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR(100)")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        [Column(TypeName = "VARCHAR(500)")]
        public string? Descricao { get; set; }

        public int Ordem { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? AtualizadoEm { get; set; }

        public ICollection<ItemCardapio> Itens { get; set; } = new List<ItemCardapio>();
    }
}
