using GestionProductos.Core.Models;

namespace GestionProductos.Web.ViewModels;

public sealed class InventarioViewModel
{
    public string? Busqueda { get; init; }
    public string Estado { get; init; } = "todos";
    public required IReadOnlyList<Producto> Productos { get; init; }
    public required IReadOnlyList<Producto> ProductosConAlerta { get; init; }
    public int TotalProductos { get; init; }
    public long TotalUnidades { get; init; }
    public decimal ValorInventario { get; init; }
    public int SinExistencias { get; init; }
    public int BajoStock { get; init; }
    public int Disponibles { get; init; }
}
