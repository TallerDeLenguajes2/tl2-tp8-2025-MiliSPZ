namespace Sistema;

public class Presupuestos
{
    public int idPresupuesto { get; set; }
    public string? nombreDestinatario { get; set; }
    public DateOnly fechaCreacion { get; set; }
    public List<PresupuestosDetalle> detalle { get; set; }

    // Contructores
    public Presupuestos(int idPresupuesto, string? nombreDestinatario, DateOnly fechaCreacion, List<PresupuestosDetalle> detalle)
    {
        this.idPresupuesto = idPresupuesto;
        this.nombreDestinatario = nombreDestinatario;
        this.fechaCreacion = fechaCreacion;
        this.detalle = detalle;
    }

    public Presupuestos() { }

    // Metodos
    public int MontoPresupuestos() // si querés mantener int
    {
        if (detalle == null || detalle.Count == 0) return 0;
        // Operador * primero; poné paréntesis para dejar claro
        return detalle.Sum(d => d.producto != null ? d.producto.precio * d.cantidad : 0);
    }

    public decimal MontoConIva(decimal iva = 0.21m)
    {
        return Math.Round(MontoPresupuestos() * (1 + iva), 2);
    }

    public int CantidadProductos()
    {
        if (detalle == null || detalle.Count == 0) return 0;
        return detalle.Sum(d => d.cantidad);
    }

}