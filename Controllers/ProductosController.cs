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
        if (!ModelState.IsValid)
        {
            return View("Create"); 
        }
        productoRepository.createProducto(productoNuevo);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto = productoRepository.getDetallesProducto(id);
        if (producto is null) RedirectToAction("Index");
        return View(producto);
    }

    [HttpPost]
    public IActionResult Edit(Productos producto)
    {
        productoRepository.updateProducto(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        productoRepository.deleteProducto(id);
        return RedirectToAction("Index");
    }
}