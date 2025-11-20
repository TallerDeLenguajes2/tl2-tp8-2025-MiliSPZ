using Microsoft.AspNetCore.Mvc;
using Sistema.Web.ViewModels;
using Sistema.Web.Models;
using Sistema.Web.Repositories;

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
        var producto = new ProductoViewModel();
        return View(producto);
    }

    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        if (!ModelState.IsValid)
        {
            return View(productoVM);    
        }

        var productoNuevo = new Productos
        {
            descripcion = productoVM.Descripcion,
            precio = productoVM.Precio
        };

        productoRepository.createProducto(productoNuevo);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto = productoRepository.getDetallesProducto(id);
        if (producto is null)
        {
            Console.WriteLine($"ID: {producto.idProducto}, Desc: {producto.descripcion}, Precio: {producto.precio}");

            return RedirectToAction("Index");
        }

        var editProducto = new ProductoViewModel
        {
            idProducto = producto.idProducto,
            Descripcion = producto.descripcion,
            Precio = producto.precio
        };

        return View(editProducto);
    }

    [HttpPost]
    public IActionResult Edit( ProductoViewModel productoVM)
    {   
        Console.WriteLine($"ID: {productoVM.idProducto}, Desc: {productoVM.Descripcion}, Precio: {productoVM.Precio}");

        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }

        var producto = new Productos
        {
            idProducto = productoVM.idProducto,
            descripcion = productoVM.Descripcion,
            precio = productoVM.Precio
        };

        productoRepository.updateProducto(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var producto = productoRepository.getDetallesProducto(id);
        if (producto is null) return RedirectToAction("Index");
        return View(producto);
    }

    [HttpPost]
    public IActionResult Delete(Productos producto)
    {
        productoRepository.deleteProducto(producto.idProducto);
        return RedirectToAction("Index");
    }
}