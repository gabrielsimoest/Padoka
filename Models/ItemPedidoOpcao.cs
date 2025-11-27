using System.ComponentModel.DataAnnotations.Schema;

namespace Padoka.Models
{
    public class ItemPedidoOpcao
    {
        public long Id { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal PrecoAdicional { get; set; }

        public long ItemPedidoId { get; set; }
        [ForeignKey("ItemPedidoId")]
        public ItemPedido ItemPedido { get; set; } = null!;

        public long OpcaoAdicionalId { get; set; }
        [ForeignKey("OpcaoAdicionalId")]
        public OpcaoAdicional OpcaoAdicional { get; set; } = null!;
    }
}
