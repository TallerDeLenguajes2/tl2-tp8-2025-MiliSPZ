namespace Sistema.Web.Repositories;

using Sistema.Web.Interfaces;
using Sistema.Web.Models;

using Microsoft.Data.Sqlite;

public class UsuarioRepository : IUserRepository
{
    string connectionString = "Data Source=Tienda.db";
    public Usuario getUser(string usuario, string contrasena)
    {
        
        Usuario user = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            //Consulta SQL que busca por Usuario Y Contrasena
            const string sql = @"SELECT Id, Nombre, User, Pass, Rol FROM Usuarios WHERE User = @Usuario AND Pass = @Contrasena";
            
            using (var command = new SqliteCommand(sql, connection))
            {

                // Se usan parámetros para prevenir inyección SQL
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contrasena",
                contrasena);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // Si el lector encuentra una fila, el usuario existe y las credenciales son correctas
                    user = new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        User = reader.GetString(2),
                        Pass = reader.GetString(3),
                        Rol = reader.GetString(4)
                    };
                }
                return user;
            }
        }
    }

}