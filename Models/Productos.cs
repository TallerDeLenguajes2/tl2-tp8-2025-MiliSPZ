using System.ComponentModel.DataAnnotations;
namespace Sistema;

public class Productos
{
    public int idProducto { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? descripcion { get; set; }

    [Range(1, 2_000_000_000, ErrorMessage = "El precio debe ser mayor que 0")]
    public int precio { get; set; }

    // Constructores
    public Productos(int dProducto, string? descripcion, int precio)
    {
        this.idProducto = dProducto;
        this.descripcion = descripcion;
        this.precio = precio;
    }

    public Productos() { }
}