namespace Sistema.Web.Interfaces;
using Sistema.Web.Models;
public interface IUserRepository
{
    // Retorna el objeto Usuario si las credenciales son válidas, sino null.
    Usuario getUser(string username, string password);
}
