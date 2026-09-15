using Microsoft.Data.Sqlite;

namespace GestionProductos.Data;

public sealed class BaseDatos
{
    public string Ruta { get; }

    public BaseDatos(string? ruta = null)
    {
        Ruta = Path.GetFullPath(ruta ?? Path.Combine(Environment.CurrentDirectory, "productos.db"));
        var directorio = Path.GetDirectoryName(Ruta);
        if (!string.IsNullOrWhiteSpace(directorio))
            Directory.CreateDirectory(directorio);
    }

    public SqliteConnection AbrirConexion()
    {
        var conexion = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = Ruta }.ToString());
        conexion.Open();
        return conexion;
    }

    public void Inicializar()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = """
            CREATE TABLE IF NOT EXISTS Productos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL CHECK (length(trim(Nombre)) > 0),
                PrecioCentavos INTEGER NOT NULL CHECK (PrecioCentavos > 0),
                Cantidad INTEGER NOT NULL CHECK (Cantidad >= 0)
            );
            """;
        comando.ExecuteNonQuery();
    }
}
