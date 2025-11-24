namespace Sistema.Web.Repositories;

using Sistema.Web.Interfaces;
using Sistema.Web.Models;

using Microsoft.Data.Sqlite;

public class UsuarioRepository : IUserRepository
{
    string connectionString = "Data Source=Tienda.db";
    public Usuarios getUser(string usuario, string contrasena)
    {
        
        Usuarios user = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
           
            const string sql = @"SELECT Id, Nombre, User, Pass, Rol FROM Usuarios WHERE User = @Usuario AND Pass = @Contrasena";
            
            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contrasena",
                contrasena);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    
                    user = new Usuarios
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