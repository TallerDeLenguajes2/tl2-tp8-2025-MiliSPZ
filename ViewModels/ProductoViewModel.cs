using System.ComponentModel.DataAnnotations;
namespace Sistema.Web.ViewModels;

public class ProductoViewModel
{
    public int idProducto { get; set; }

    [Display(Name = "Descripción del Producto")]
    [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
    public string? Descripcion { get; set; }
    // Validación: Requerido y debe ser positivo
    [Display(Name = "Precio Unitario")]
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
    public int Precio { get; set; }
}