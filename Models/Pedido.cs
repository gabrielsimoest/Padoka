using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padoka.Models
{
    public class Pedido
    {
        public long Id { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public string NumeroPedido { get; set; } = string.Empty;

        [StringLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public string? Mesa { get; set; }

        [StringLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public StatusPedido Status { get; set; } = StatusPedido.Recebido;

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal ValorTotal { get; set; }

        [StringLength(500)]
        [Column(TypeName = "VARCHAR(500)")]
        public string? Observacoes { get; set; }

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? AtualizadoEm { get; set; }

        public long UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;

        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public ICollection<HistoricoStatusPedido> HistoricoStatus { get; set; } = new List<HistoricoStatusPedido>();
    }

    public enum StatusPedido
    {
        Recebido = 0,
        EmPreparo = 1,
        Pronto = 2,
        Entregue = 3
    }
}
