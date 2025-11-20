using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_MiliSPZ.Models;
using Sistema.Web.Interfaces;

namespace tl2_tp8_2025_MiliSPZ.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IAuthenticationService _authService;

    public HomeController(ILogger<HomeController> logger, IAuthenticationService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    public IActionResult Index()
    {
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (_authService.HasAccessLevel("Administrador") || _authService.HasAccessLevel("Cliente") )
        {
            //si es es valido entra sino vuelve a login
            return View();
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
