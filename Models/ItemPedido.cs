using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padoka.Models
{
    public class ItemPedido
    {
        public long Id { get; set; }
        public int Quantidade { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal PrecoUnitario { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal PrecoTotal { get; set; }

        [StringLength(300)]
        [Column(TypeName = "VARCHAR(300)")]
        public string? Observacoes { get; set; }

        public long PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; } = null!;

        public long ItemCardapioId { get; set; }
        [ForeignKey("ItemCardapioId")]
        public ItemCardapio ItemCardapio { get; set; } = null!;

        public ICollection<ItemPedidoOpcao> OpcoesAdicionais { get; set; } = new List<ItemPedidoOpcao>();
    }
}
