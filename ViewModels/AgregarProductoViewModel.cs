namespace Sistema.Web.ViewModels;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class AgregarProductoViewModel
{
    public int idPresupuesto { get; set; }

    [Display(Name = "Producto")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un producto.")]
    public int IdProducto { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Cantidad { get; set; }

    /* preguntar porque me pide validacion */
    [ValidateNever]
    public IEnumerable<SelectListItem> ListaProductos { get; set; }
}