using Microsoft.AspNetCore.Http;
using Sistema.Web.Interfaces;
using Sistema.Web.Models;
namespace Sistema.Web.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _usuarioRepository;

    public AuthenticationService(IHttpContextAccessor httpContextAccessor, IUserRepository usuarioRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _usuarioRepository = usuarioRepository;
    }

    public bool Login(string user, string pass)
    {
    
        var usuario = _usuarioRepository.getUser(user, pass); 

        if (usuario == null)
        {
            
            return false;
        }

        var httpContext = _httpContextAccessor.HttpContext!;
        httpContext.Session.SetString("UserName", usuario.User);
        httpContext.Session.SetString("Role", usuario.Rol); 
        return true;
    }

    public bool IsAuthenticated()
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        return !string.IsNullOrEmpty(httpContext.Session.GetString("UserName"));
    }

    public bool HasAccessLevel(string role)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        var storedRole = httpContext.Session.GetString("Role");

        return string.Equals(storedRole, role, StringComparison.OrdinalIgnoreCase);
    }

    public void Logout()
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        httpContext.Session.Clear();
    }
}
