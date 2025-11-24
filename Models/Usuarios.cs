namespace Sistema.Web.Models;

public class Usuarios
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string User { get; set; }
    public string Pass { get; set; }
    public string Rol { get; set; }

    // Constructores
    public Usuarios(int id, string nombre, string user, string pass, string rol)
    {
        Id = id;
        Nombre = nombre;
        User = user;
        Pass = pass;
        Rol = rol;
    }

    public Usuarios() { }
}