namespace Sistema.Web.Interfaces;
public interface IAuthenticationService
{
    bool Login(string user, string pass);
    bool IsAuthenticated();
    bool HasAccessLevel(string role);
    void Logout();
}
