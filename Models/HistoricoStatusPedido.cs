using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padoka.Models
{
    public class HistoricoStatusPedido
    {
        public long Id { get; set; }

        [StringLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public StatusPedido StatusAnterior { get; set; }

        [StringLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public StatusPedido StatusNovo { get; set; }

        public DateTime AlteradoEm { get; set; } = DateTime.UtcNow;

        public long PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; } = null!;

        public long? AlteradoPorId { get; set; }
        [ForeignKey("AlteradoPorId")]
        public Usuario? AlteradoPor { get; set; }
    }
}
