using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appUPN.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("userid")]
        public int UserId { get; set; }

        [Required]
        [Column("nombre")]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [StringLength(120)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("passwordhash")]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("telefono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Column("direccion")]
        [StringLength(200)]
        public string? Direccion { get; set; }

        [Required]
        [Column("rol")]
        [StringLength(20)]
        public string Rol { get; set; } = "Cliente";

        [Column("fecharegistro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}
