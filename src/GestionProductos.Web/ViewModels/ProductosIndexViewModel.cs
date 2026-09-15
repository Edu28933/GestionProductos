using GestionProductos.Core.Models;

namespace GestionProductos.Web.ViewModels;

public sealed class ProductosIndexViewModel
{
    public string? Busqueda { get; init; }
    public required IReadOnlyList<Producto> Productos { get; init; }
    public int TotalProductos { get; init; }
    public long TotalUnidades { get; init; }
    public decimal ValorExistencias { get; init; }
}
