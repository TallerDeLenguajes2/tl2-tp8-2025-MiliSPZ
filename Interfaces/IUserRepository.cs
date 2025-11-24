namespace Sistema.Web.Interfaces;
using Sistema.Web.Models;
public interface IUserRepository
{

    Usuarios getUser(string username, string password);
}
