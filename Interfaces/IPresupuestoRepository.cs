namespace Sistema.Web.Interfaces;
using Sistema.Web.Models;

public interface IPresupuestoRepository
{
    bool createPresupuesto(Presupuestos presupuesto);
    bool updatePresupuesto(Presupuestos Presupuesto);
    public List<Presupuestos> getPresupuestos();
    public Presupuestos? getDetallesPresupuesto(int id);
    public bool agregarDetalle(int idPresupuesto, int idProducto, int cantidad);
    public bool deletePresupuesto(int id);
}