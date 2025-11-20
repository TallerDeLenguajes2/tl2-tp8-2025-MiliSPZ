using Microsoft.AspNetCore.Mvc;
using Sistema.Web.ViewModels;
using Sistema.Web.Models;
using Sistema.Web.Repositories;
using Sistema.Web.Interfaces;

public class ProductosController: Controller
{
    private IProductoRepository productoRepository;
    private IAuthenticationService _authService;
    public ProductosController(IProductoRepository prodRepo, IAuthenticationService authService)
    {
        productoRepository = prodRepo;
        _authService = authService;
    }

    //A partir de aquí van todos los Action Methods (Get, Post,etc.)
    public IActionResult AccesoDenegado()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        
        List<Productos> productos = productoRepository.getProductos();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        var producto = new ProductoViewModel();
        return View(producto);
    }

    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        var producto = productoRepository.getDetallesProducto(id);
        if (producto is null) return RedirectToAction("Index");
        return View(producto);
    }

    [HttpPost]
    public IActionResult Delete(Productos producto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        
        productoRepository.deleteProducto(producto.idProducto);
        return RedirectToAction("Index");
    }

    // Private

    private IActionResult CheckAdminPermissions()
    {
        // 1. No logueado? -> vuelve al login
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. No es Administrador? -> Da Error
        if (!_authService.HasAccessLevel("Administrador"))
        {
        // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
            return RedirectToAction(nameof(AccesoDenegado));
        }
        return null; // Permiso concedido
    }
}