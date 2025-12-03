using System.ComponentModel.DataAnnotations;

namespace appUPN.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(120)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirma tu contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Teléfono inválido")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }
    }
}
