using System.ComponentModel.DataAnnotations;

namespace ClubesDeportivos.Models;

public class Miembro
{
    public int MiembroId { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, ErrorMessage = "Máximo 60 caracteres.")]
    public string Nombre { get; set; } = "";

    [Display(Name = "Apellido")]
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(60, ErrorMessage = "Máximo 60 caracteres.")]
    public string Apellido { get; set; } = "";

    [Display(Name = "Email")]
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Email { get; set; } = "";

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^\d{7,10}$", ErrorMessage = "El teléfono debe tener entre 7 y 10 dígitos.")]
    public string Telefono { get; set; } = "";
}
