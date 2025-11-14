namespace Sistema.Web.Models;

public class Productos
{
    public int idProducto { get; set; }
    public string? descripcion { get; set; }
    public int precio { get; set; }

    // Constructores
    public Productos(int dProducto, string? descripcion, int precio)
    {
        this.idProducto = dProducto;
        this.descripcion = descripcion;
        this.precio = precio;
    }
    
    public Productos() {}
}