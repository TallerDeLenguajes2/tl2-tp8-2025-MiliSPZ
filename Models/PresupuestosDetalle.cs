namespace Sistema.Web.Models;

public class PresupuestosDetalle
{
    public Productos producto { get; set; }
    public int cantidad { get; set; }

    // Contructores
    public PresupuestosDetalle(Productos producto, int cantidad)
    {
        this.producto = producto;
        this.cantidad = cantidad;
    }

    public PresupuestosDetalle() {}
}