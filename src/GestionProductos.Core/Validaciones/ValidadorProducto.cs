namespace GestionProductos.Core.Validaciones;

public static class ValidadorProducto
{
    public static void ValidarNombre(string? nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
    }

    public static void ValidarPrecio(decimal precio)
    {
        if (precio <= 0)
            throw new ArgumentOutOfRangeException(nameof(precio), "El precio debe ser mayor que cero");
    }

    public static void ValidarCantidad(int cantidad)
    {
        if (cantidad < 0)
            throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad no puede ser negativa.");
    }

    public static void ValidarProducto(string? nombre, decimal precio, int cantidad)
    {
        ValidarNombre(nombre);
        ValidarPrecio(precio);
        ValidarCantidad(cantidad);
    }
}
