using Microsoft.AspNetCore.Mvc;
namespace Sistema;

public class ProductosController: Controller
{
    private ProductosRepository productoRepository;
    public ProductosController()
    {
        productoRepository = new ProductosRepository();
    }

    //A partir de aquí van todos los Action Methods (Get, Post,etc.)

    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = productoRepository.getProductos();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var producto = new Productos();
        return View(producto);
    }

    [HttpPost]
    public IActionResult Create(Productos productoNuevo)
    {
        productoRepository.createProducto(productoNuevo);
        return RedirectToAction("Index");
    }
}