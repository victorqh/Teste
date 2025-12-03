using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appUPN.Models
{
    [Table("categorias")]
    public class Categoria
    {
        [Key]
        [Column("categoriaid")]
        public int CategoriaId { get; set; }

        [Required]
        [Column("nombre")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Column("descripcion")]
        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
