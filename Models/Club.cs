using System.ComponentModel.DataAnnotations;

namespace ClubesDeportivos.Models;

public class Club
{
    public int ClubId { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Nombre { get; set; } = "";

    [Display(Name = "Deporte")]
    [Required(ErrorMessage = "El deporte es obligatorio.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string Deporte { get; set; } = "";

    [Display(Name = "Ubicación")]
    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
    public string Ubicacion { get; set; } = "";

    [Display(Name = "Fecha de fundación")]
    [Required(ErrorMessage = "La fecha de fundación es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime? FechaFundacion { get; set; }

    public int TotalMiembros { get; set; }
}
