using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appTiendaUPN.Models
{
    [Table("carritos")]
    public class Carrito
    {
        [Key]
        [Column("carritoid")]
        public int CarritoId { get; set; }

        [Required]
        [Column("userid")]
        public int UserId { get; set; }

        [Column("fechacreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación con Usuario
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Relación con Items del Carrito
        public ICollection<CarritoItem> Items { get; set; } = new List<CarritoItem>();
    }
}
