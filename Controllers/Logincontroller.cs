using Microsoft.AspNetCore.Mvc;
using Sistema.Web.Interfaces;
using Sistema.Web.ViewModels;

public class LoginController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    public LoginController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (string.IsNullOrEmpty(model.User) || string.IsNullOrEmpty(model.Pass))
        {
            model.ErrorMessage = "Debe ingresar usuario y contraseña.";
            return View("Index", model);
        }

        if (_authenticationService.Login(model.User, model.Pass))
        {
            return RedirectToAction("Index", "Home");
        }

        model.ErrorMessage = "Credenciales inválidas.";
        return View("Index", model);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        _authenticationService.Logout();
        return RedirectToAction("Index");
    }
}
