using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appTiendaUPN.Models
{
    [Table("productos")]
    public class Producto
    {
        [Key]
        [Column("productoid")]
        public int ProductoId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Required]
        [Column("precio", TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Column("precioanterior", TypeName = "decimal(18,2)")]
        public decimal? PrecioAnterior { get; set; }

        [Required]
        [Column("stock")]
        public int Stock { get; set; }

        [MaxLength(500)]
        [Column("imagenurl")]
        public string? ImagenUrl { get; set; }

        [Required]
        [Column("categoriaid")]
        public int CategoriaId { get; set; }

        [Column("estaactivo")]
        public bool EstaActivo { get; set; } = true;

        [Column("esoferta")]
        public bool EsOferta { get; set; } = false;

        [Column("fechacreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación con Categoría
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }
    }
}
