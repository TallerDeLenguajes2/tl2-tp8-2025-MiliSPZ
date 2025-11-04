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

    // Obtener detalles de un Presupuesto por su ID. (recibe un Id y devuelve u Presupuesto con sus productos y cantidades)

    public Presupuestos getDetallesPresupuesto(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT p.idPresupuesto, p.NombreDestinatario, p.FechaCreacion, pr.idProducto, pr.Descripcion, pr.Precio, pd.Cantidad FROM Presupuestos AS p INNER JOIN PresupuestosDetalle AS pd ON pd.idPresupuesto = p.idPresupuesto INNER JOIN Productos AS pr ON pr.idProducto = pd.idProducto WHERE p.idPresupuesto = @id;";
            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    var presupuestoEncontrado = new Presupuestos();
                    while (reader.Read())
                    {
                        presupuestoEncontrado.idPresupuesto = Convert.ToInt32(reader["idProducto"]);
                        presupuestoEncontrado.nombreDestinatario = reader["NombreDestinatario"].ToString();

                        var iFecha = reader.GetOrdinal("FechaCreacion");
                        presupuestoEncontrado.fechaCreacion = DateOnly.FromDateTime(reader.GetDateTime(iFecha));

                        presupuestoEncontrado.detalle = new List<PresupuestosDetalle>();

                        var prod = new Productos
                        {
                            idProducto = reader.GetInt32(reader.GetOrdinal("idProducto")),
                            descripcion = reader["ProductoDescripcion"]?.ToString(),
                            // Sugerencia: precio debería ser decimal; si por ahora es int, convertimos tal cual
                            precio = Convert.ToInt32(reader["ProductoPrecio"])
                        };

                        presupuestoEncontrado.detalle.Add(new PresupuestosDetalle
                        {
                            producto = prod,
                            cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad"))
                        });
                    }

                    return presupuestoEncontrado;
                }

            }

        }
    }
    
    public bool AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        if (cantidad <= 0) throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser > 0.");

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        using var tx = connection.BeginTransaction();

        // (Opcional) validar que exista el presupuesto
        using (var chk = new SqliteCommand("SELECT 1 FROM Presupuestos WHERE idPresupuesto=@id LIMIT 1", connection, tx))
        {
            chk.Parameters.AddWithValue("@id", idPresupuesto);
            var exists = chk.ExecuteScalar();
            if (exists is null)
                throw new InvalidOperationException($"No existe el presupuesto {idPresupuesto}.");
        }

        // ¿Ya existe ese producto en el detalle? -> UPDATE, si no -> INSERT
        int? cantidadActual = null;
        using (var sel = new SqliteCommand(
            "SELECT Cantidad FROM PresupuestosDetalle WHERE idPresupuesto=@idP AND idProducto=@idPr LIMIT 1",
            connection, tx))
        {
            sel.Parameters.AddWithValue("@idP", idPresupuesto);
            sel.Parameters.AddWithValue("@idPr", idProducto);
            var r = sel.ExecuteScalar();
            if (r != null && r != DBNull.Value) cantidadActual = Convert.ToInt32(r);
        }

        if (cantidadActual.HasValue)
        {
            using var upd = new SqliteCommand(
                "UPDATE PresupuestosDetalle SET Cantidad = Cantidad + @cant WHERE idPresupuesto=@idP AND idProducto=@idPr",
                connection, tx);
            upd.Parameters.AddWithValue("@cant", cantidad);
            upd.Parameters.AddWithValue("@idP", idPresupuesto);
            upd.Parameters.AddWithValue("@idPr", idProducto);
            upd.ExecuteNonQuery();
        }
        else
        {
            using var ins = new SqliteCommand(
                "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idP, @idPr, @cant)",
                connection, tx);
            ins.Parameters.AddWithValue("@idP", idPresupuesto);
            ins.Parameters.AddWithValue("@idPr", idProducto);
            ins.Parameters.AddWithValue("@cant", cantidad);
            ins.ExecuteNonQuery();
        }

        tx.Commit();
        return true;
    }

    // 2) Eliminar un presupuesto por ID (borra primero el detalle para evitar FK issues)
    public bool EliminarPresupuestoPorId(int idPresupuesto)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        using var tx = connection.BeginTransaction();

        // Si tenés FK con ON DELETE CASCADE podrías omitir este DELETE
        using (var delDet = new SqliteCommand(
            "DELETE FROM PresupuestosDetalle WHERE idPresupuesto=@id",
            connection, tx))
        {
            delDet.Parameters.AddWithValue("@id", idPresupuesto);
            delDet.ExecuteNonQuery();
        }

        int borrados;
        using (var del = new SqliteCommand(
            "DELETE FROM Presupuestos WHERE idPresupuesto=@id",
            connection, tx))
        {
            del.Parameters.AddWithValue("@id", idPresupuesto);
            borrados = del.ExecuteNonQuery();
        }

        if (borrados == 0)
        {
            tx.Rollback();
            return false; // no existía el presupuesto
        }

        tx.Commit();
        return true;
    }
}