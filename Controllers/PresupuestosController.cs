using Microsoft.AspNetCore.Mvc;
using Sistema.Web.Repository;
using Sistema.Web.ViewModels;
using Sistema.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


public class PresupuestosController: Controller
{
    private readonly PresupuestosRepository presupuestoRepository = new PresupuestosRepository();

    private readonly ProductosRepository productoRepository = new ProductosRepository();

    //A partir de aquí van todos los Action Methods (Get, Post,etc.)
    
    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        // 1. Obtener los productos para el SelectList
        List<Productos> productos = productoRepository.getProductos();

        // 2. Crear el ViewModel
        AgregarProductoViewModel model = new AgregarProductoViewModel
        {
            idPresupuesto = id, // Pasamos el ID del presupuesto actual

            // 3. Crear el SelectList
            ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
       // 1. Chequeo de Seguridad para la Cantidad
        if (!ModelState.IsValid)
        {
        // LÓGICA CRÍTICA DE RECARGA: Si falla la validación,
        // debemos recargar el SelectList porque se pierde en el POST.
        var productos = productoRepository.getProductos();
        model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");

        // Devolvemos el modelo con los errores y el dropdown recargado
        return View(model);
        }

        // 2. Si es VÁLIDO: Llamamos al repositorio para guardar la relación
        presupuestoRepository.agregarDetalle(model.idPresupuesto, model.IdProducto, model.Cantidad);

        // 3. Redirigimos al detalle del presupuesto
        return RedirectToAction(nameof(Details), new { id = model.idPresupuesto }); 
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> Presupuestos = presupuestoRepository.getPresupuestos();
        return View(Presupuestos);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var presupuesto = presupuestoRepository.getDetallesPresupuesto(id);
        if (presupuesto is null) return RedirectToAction("Index");
        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var presupuesto = new Presupuestos
        {
            fechaCreacion = DateOnly.FromDateTime(DateTime.Today)
        };
        
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Create(Presupuestos presupuestoNuevo)
    {
        if (!ModelState.IsValid)
        {
            return View("Create"); 
        }
        presupuestoRepository.createPresupuesto(presupuestoNuevo);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = presupuestoRepository.getDetallesPresupuesto(id);
        if (presupuesto is null) return RedirectToAction("Index");
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Edit(Presupuestos presupuesto)
    {
        presupuestoRepository.updatePresupuesto(presupuesto);
        return RedirectToAction("Index");
    } 

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var presupuesto = presupuestoRepository.getDetallesPresupuesto(id);
        if (presupuesto is null) return RedirectToAction("Index");
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Delete(Presupuestos presupuesto)
    {
        presupuestoRepository.deletePresupuesto(presupuesto.idPresupuesto);
        return RedirectToAction("Index");
    }
}