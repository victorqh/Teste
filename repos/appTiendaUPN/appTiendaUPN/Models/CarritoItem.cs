using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appTiendaUPN.Models
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
        public int Cantidad { get; set; }

        [Required]
        [Column("precio", TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        // Relaciones
        [ForeignKey("CarritoId")]
        public Carrito? Carrito { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }
    }
}
