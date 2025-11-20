namespace Sistema.Web.Interfaces;
using Sistema.Web.Models;

public interface IProductoRepository
{
    bool createProducto(Productos producto);
    bool updateProducto(Productos producto);
    public List<Productos> getProductos();
    public Productos getDetallesProducto(int id);
    bool deleteProducto(int idProducto);

}