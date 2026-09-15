using GestionProductos.Core.Models;
using GestionProductos.Core.Validaciones;
using Microsoft.Data.Sqlite;

namespace GestionProductos.Data;

public sealed class ProductoRepository(BaseDatos baseDatos)
{
    private static long ACentavos(decimal precio)
    {
        ValidadorProducto.ValidarPrecio(precio);
        if (decimal.Round(precio, 2) != precio)
            throw new ArgumentException("El precio admite como máximo dos decimales.", nameof(precio));
        return checked((long)(precio * 100m));
    }

    private static Producto Leer(SqliteDataReader lector) => new(
        lector.GetInt32(0), lector.GetString(1), lector.GetInt64(2) / 100m, lector.GetInt32(3));

    public int Registrar(string nombre, decimal precio, int cantidad)
    {
        ValidadorProducto.ValidarProducto(nombre, precio, cantidad);
        long centavos = ACentavos(precio);
        using var conexion = baseDatos.AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "INSERT INTO Productos (Nombre, PrecioCentavos, Cantidad) VALUES ($nombre, $precio, $cantidad); SELECT last_insert_rowid();";
        comando.Parameters.AddWithValue("$nombre", nombre.Trim());
        comando.Parameters.AddWithValue("$precio", centavos);
        comando.Parameters.AddWithValue("$cantidad", cantidad);
        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public List<Producto> Listar()
    {
        var productos = new List<Producto>();
        using var conexion = baseDatos.AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Nombre, PrecioCentavos, Cantidad FROM Productos ORDER BY Id";
        using var lector = comando.ExecuteReader();
        while (lector.Read()) productos.Add(Leer(lector));
        return productos;
    }

    public Producto? Buscar(int id)
    {
        using var conexion = baseDatos.AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Nombre, PrecioCentavos, Cantidad FROM Productos WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);
        using var lector = comando.ExecuteReader();
        return lector.Read() ? Leer(lector) : null;
    }

    public bool Actualizar(int id, string nombre, decimal precio, int cantidad)
    {
        ValidadorProducto.ValidarProducto(nombre, precio, cantidad);
        long centavos = ACentavos(precio);
        using var conexion = baseDatos.AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Productos SET Nombre = $nombre, PrecioCentavos = $precio, Cantidad = $cantidad WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);
        comando.Parameters.AddWithValue("$nombre", nombre.Trim());
        comando.Parameters.AddWithValue("$precio", centavos);
        comando.Parameters.AddWithValue("$cantidad", cantidad);
        return comando.ExecuteNonQuery() > 0;
    }

    public bool Eliminar(int id)
    {
        using var conexion = baseDatos.AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Productos WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);
        return comando.ExecuteNonQuery() > 0;
    }
}
