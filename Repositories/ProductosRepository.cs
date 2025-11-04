using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace Sistema;

public class ProductosRepository
{
    string connectionString = "Data Source=Tienda.db";

    public bool createProducto(Productos producto)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Productos (Descripcion, Precio) Values (@desc, @prec)";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@desc", producto.descripcion);
                    command.Parameters.AddWithValue("@prec", producto.precio);

                    int rows = command.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
        catch
        {

            return false;
        }

    }

    public bool updateProducto(int id, Productos producto)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "UPDATE Productos SET Descripcion = @desc, Precio = @prec WHERE idProdcuto = @id";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@desc", producto.descripcion);
                    command.Parameters.AddWithValue("@prec", producto.precio);
                    command.Parameters.AddWithValue("@id", producto.idProducto);

                    int rows = command.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
        catch
        {

            return false;
        }
    }

    public List<Productos> getProductos()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT * FROM Productos";

            using (var command = new SqliteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var ListaProductos = new List<Productos>();
                    while (reader.Read())
                    {
                        var producto = new Productos();
                        producto.idProducto = Convert.ToInt32(reader["idProducto"]);
                        producto.descripcion = reader["Descripcion"].ToString();
                        producto.precio = Convert.ToInt32(reader["Precio"]);

                        ListaProductos.Add(producto);
                    }

                    return ListaProductos;
                }
            }

        }

    }

    public Productos getDetallesProducto(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT * FROM Productos WHERE idProducto = @id";
            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    var productoEncontrado = new Productos();
                    while (reader.Read())
                    {
                        productoEncontrado.idProducto = Convert.ToInt32(reader["idProducto"]);
                        productoEncontrado.descripcion = reader["Descripcion"].ToString();
                        productoEncontrado.precio = Convert.ToInt32(reader["Precio"]);
                    }

                    return productoEncontrado;
                }
            }
        }
    }

    public bool deleteProducto(int id)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Productos WHERE idProducto = @id";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int rows = command.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
        catch
        {

            return false;
        }
    }
}