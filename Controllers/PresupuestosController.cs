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
        var productos = productoRepository.getProductos();
        var model = new AgregarProductoViewModel
        {
            idPresupuesto = id,
            ListaProductos = new SelectList(productos, "idProducto", "descripcion")
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var productos = productoRepository.getProductos();
            model.ListaProductos = new SelectList(productos, "idProducto", "descripcion");

            // Guardar errores para mostrarlos en la vista (temporal)
            TempData["ModelErrors"] = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            return View(model);
        }

        // Debug: inspeccionar valores recibidos en Output (quitar luego)
        System.Diagnostics.Debug.WriteLine($"AgregarProducto POST: idPresupuesto={model.idPresupuesto}, IdProducto={model.IdProducto}, Cantidad={model.Cantidad}");

        presupuestoRepository.agregarDetalle(model.idPresupuesto, model.IdProducto, model.Cantidad);
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

        var ar = new System.Globalization.CultureInfo("es-AR");
        ViewBag.FechaLarga = presupuesto.fechaCreacion
            .ToDateTime(System.TimeOnly.MinValue)
            .ToString("d 'de' MMMM 'de' yyyy", ar);

        var total = presupuesto.MontoPresupuestos();
        ViewBag.TotalFmt = string.Format(ar, "{0:C0}", total);

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