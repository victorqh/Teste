using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appUPN.Models
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

        // Navegación
        public virtual ICollection<CarritoItem> Items { get; set; } = new List<CarritoItem>();
    }
}
