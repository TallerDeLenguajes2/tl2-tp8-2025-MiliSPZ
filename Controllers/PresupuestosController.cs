using Microsoft.AspNetCore.Mvc;
namespace Sistema;

public class PresupuestosController: Controller
{
    private PresupuestosRepository presupuestoRepository;
    public PresupuestosController()
    {
        presupuestoRepository = new PresupuestosRepository();
    }

    //A partir de aquí van todos los Action Methods (Get, Post,etc.)

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> Presupuestos = presupuestoRepository.getPresupuestos();
        return View(Presupuestos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var presupuesto = new Presupuestos();
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

   /*  [HttpPost]
    public IActionResult Edit(Presupuestos presupuesto)
    {
        presupuestoRepository.updatePresupuesto(presupuesto);
        return RedirectToAction("Index");
    } */

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