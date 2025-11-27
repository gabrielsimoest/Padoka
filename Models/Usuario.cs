using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padoka.Models
{
    public class Usuario
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR(100)")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Column(TypeName = "VARCHAR(150)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        [Column(TypeName = "VARCHAR(256)")]
        public string SenhaHash { get; set; } = string.Empty;

        [StringLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public TipoUsuario Tipo { get; set; } = TipoUsuario.Cliente;

        public bool Ativo { get; set; } = true;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoAcesso { get; set; }

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<HistoricoStatusPedido> AlteracoesStatus { get; set; } = new List<HistoricoStatusPedido>();
    }

    public enum TipoUsuario
    {
        Cliente = 0,
        Administrador = 1
    }
}
