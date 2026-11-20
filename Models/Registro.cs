using System.ComponentModel.DataAnnotations;

namespace ClubesDeportivos.Models;

public class Registro
{
    public int RegistroId { get; set; }

    [Display(Name = "Club")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un club.")]
    public int ClubId { get; set; }

    [Display(Name = "Miembro")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un miembro.")]
    public int MiembroId { get; set; }

    [Display(Name = "Fecha de registro")]
    [Required(ErrorMessage = "La fecha de registro es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime? FechaRegistro { get; set; }

    public string Club { get; set; } = "";
    public string Deporte { get; set; } = "";
    public string Miembro { get; set; } = "";
    public string Email { get; set; } = "";
}
