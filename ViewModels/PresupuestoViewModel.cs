namespace Sistema.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

public class PresupuestoViewModel
{
    public int idPresupuesto { get; set; }

    [Display(Name = "Nombre o Email del Destinatario")]
    [Required(ErrorMessage = "El nombre o email es obligatorio.")]
    public string? NombreDestinatario { get; set; }

    [Display(Name = "Fecha de Creación")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateOnly FechaCreacion { get; set; }
}