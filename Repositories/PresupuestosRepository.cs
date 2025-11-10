using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
namespace Sistema;

public class PresupuestosRepository
{
    string connectionString = "Data Source=Tienda.db";

    public bool createPresupuesto(Presupuestos presupuesto)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) Values (@nombreDest, @fechaC)";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombreDest", presupuesto.nombreDestinatario);
                    command.Parameters.AddWithValue("@fechaC", presupuesto.fechaCreacion.ToString("yyyy-MM-dd"));

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

    public List<Presupuestos> getPresupuestos()
    {
        var ListaPresupuestos = new List<Presupuestos>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT * FROM Presupuestos";

            using (var command = new SqliteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var presupuesto = new Presupuestos();
                        presupuesto.idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        presupuesto.nombreDestinatario = reader["NombreDestinatario"].ToString();

                        var iFecha = reader.GetOrdinal("FechaCreacion");
                        presupuesto.fechaCreacion = DateOnly.FromDateTime(reader.GetDateTime(iFecha));

                        ListaPresupuestos.Add(presupuesto);
                    }
                }
            }
        }

        return ListaPresupuestos;
    }

    public List<PresupuestosDetalle> getDetallesPresupuesto(int id)
    {
        var listaDetalles = new List<PresupuestosDetalle>();
        using (var conexion = new SqliteConnection(connectionString))
        {
            conexion.Open();
            string consulta = "SELECT * FROM Presupuestos INNER JOIN PresupuestosDetalle using(IdPresupuesto) INNER JOIN Productos using(IdProducto) WHERE IdPresupuesto = @Id";

            var comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.Add(new SqliteParameter("@Id", id));

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                var producto = new Productos(Convert.ToInt32(lector["IdProducto"]), lector["Descripcion"].ToString(), Convert.ToInt32(lector["Precio"]));

                var detalle = new PresupuestosDetalle(producto, Convert.ToInt32(lector["Cantidad"]));

                listaDetalles.Add(detalle);
            }

        }
        return listaDetalles;

    }

    public bool agregarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        using (var conexion = new SqliteConnection(connectionString))
        {
            conexion.Open();
            string sql = "INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES (@IdPre, @IdPro, @Cant)";

            var command = new SqliteCommand(sql, conexion);

            command.Parameters.Add(new SqliteParameter("@IdPre", idPresupuesto));
            command.Parameters.Add(new SqliteParameter("@IdPro", idProducto));
            command.Parameters.Add(new SqliteParameter("@Cant", cantidad));

            int filas = command.ExecuteNonQuery();
            if (filas > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool deletePresupuesto(int id)
    {
        int rows = 0;
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql1 = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
                using (var command = new SqliteCommand(sql1, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }

                string sql2 = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";

                using (var command = new SqliteCommand(sql2, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    rows = command.ExecuteNonQuery();
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