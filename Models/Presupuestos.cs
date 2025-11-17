namespace Sistema.Web.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class Presupuestos
{
    private DateOnly dateOnly;

    public int idPresupuesto { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? nombreDestinatario { get; set; }
    public DateOnly fechaCreacion { get; set; }

    [ValidateNever]  
    public List<PresupuestosDetalle>? detalle { get; set; }

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
    public int MontoPresupuestos()
    {
        if (detalle == null || detalle.Count == 0) return 0;
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