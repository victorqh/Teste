using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appUPN.Models
{
    [Table("carritoitems")]
    public class CarritoItem
    {
        [Key]
        [Column("carritoitemid")]
        public int CarritoItemId { get; set; }

        [Required]
        [Column("carritoid")]
        public int CarritoId { get; set; }

        [Required]
        [Column("productoid")]
        public int ProductoId { get; set; }

        [Required]
        [Column("cantidad")]
        public int Cantidad { get; set; } = 1;

        [Required]
        [Column("precio")]
        public decimal Precio { get; set; }

        // Navegación
        public virtual Producto? Producto { get; set; }
        public virtual Carrito? Carrito { get; set; }
    }
}
