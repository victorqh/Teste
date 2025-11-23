using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appTiendaUPN.Models
{
    [Table("categorias")]
    public class Categoria
    {
        [Key]
        [Column("categoriaid")]
        public int CategoriaId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        // Relación con Productos
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
