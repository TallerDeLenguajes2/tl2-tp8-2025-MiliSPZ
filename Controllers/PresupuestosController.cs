using Microsoft.AspNetCore.Mvc;
using Sistema.Web.Repositories;
using Sistema.Web.ViewModels;
using Sistema.Web.Models;
using Sistema.Web.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;


public class PresupuestosController: Controller
{
    private readonly IPresupuestoRepository presupuestoRepository;

    private readonly IProductoRepository productoRepository;
    private IAuthenticationService _authService;

    public PresupuestosController(IPresupuestoRepository repo, IProductoRepository prodRepo, IAuthenticationService authService)
    {
        presupuestoRepository = repo;
        productoRepository = prodRepo;
        _authService = authService;
    }

    //A partir de aquí van todos los Action Methods (Get, Post,etc.)
    public IActionResult AccesoDenegado()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }

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

            return View(model);
        }

        presupuestoRepository.agregarDetalle(model.idPresupuesto, model.IdProducto, model.Cantidad);
        return RedirectToAction(nameof(Details), new { id = model.idPresupuesto });
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (_authService.HasAccessLevel("Administrador") || _authService.HasAccessLevel("Cliente") )
        {
            //si es es valido entra sino vuelve a login
            List<Presupuestos> Presupuestos = presupuestoRepository.getPresupuestos();
            return View(Presupuestos);
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (_authService.HasAccessLevel("Administrador") || _authService.HasAccessLevel("Cliente") )
        {
            //si es es valido entra sino vuelve a login
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
        else
        {
            return RedirectToAction("Index", "Login");
        }
  
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }

        var presupuesto = new PresupuestoViewModel
        {
            FechaCreacion = DateOnly.FromDateTime(DateTime.Today)
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
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }

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
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }
        
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