using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appUPN.Models
{
    [Table("productos")]
    public class Producto
    {
        [Key]
        [Column("productoid")]
        public int ProductoId { get; set; }

        [Required]
        [Column("nombre")]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Required]
        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("precioanterior")]
        public decimal? PrecioAnterior { get; set; }

        [Required]
        [Column("stock")]
        public int Stock { get; set; }

        [Column("imagenurl")]
        [MaxLength(500)]
        public string? ImagenUrl { get; set; }

        [Required]
        [Column("categoriaid")]
        public int CategoriaId { get; set; }

        [Column("estaactivo")]
        public bool EstaActivo { get; set; } = true;

        [Column("esoferta")]
        public bool EsOferta { get; set; } = false;

        [Column("fechacreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }
    }
}
