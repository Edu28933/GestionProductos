using GestionProductos.Core.Models;
using GestionProductos.Core.Validaciones;
using GestionProductos.Data;
using GestionProductos.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GestionProductos.Web.Controllers;

public sealed class ProductosController(ProductoRepository repositorio) : Controller
{
    public IActionResult Index(string? busqueda)
    {
        var todos = repositorio.Listar();
        var filtrados = string.IsNullOrWhiteSpace(busqueda) ? todos : todos.Where(p =>
            p.Nombre.Contains(busqueda.Trim(), StringComparison.OrdinalIgnoreCase) ||
            p.Id.ToString().Contains(busqueda.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        return View(new ProductosIndexViewModel
        {
            Busqueda = busqueda,
            Productos = filtrados,
            TotalProductos = todos.Count,
            TotalUnidades = todos.Sum(p => (long)p.Cantidad),
            ValorExistencias = todos.Sum(p => p.Precio * p.Cantidad)
        });
    }

    public IActionResult Detalle(int id)
    {
        var producto = repositorio.Buscar(id);
        return producto == null ? View("NoEncontrado") : View(producto);
    }

    [HttpGet]
    public IActionResult Crear() => View(new ProductoFormularioViewModel());

    [HttpPost]
    public IActionResult Crear(ProductoFormularioViewModel modelo)
    {
        Validar(modelo);
        if (!ModelState.IsValid) return View(modelo);
        try
        {
            repositorio.Registrar(modelo.Nombre!, modelo.Precio!.Value, modelo.Cantidad!.Value);
            TempData["Exito"] = "Producto guardado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex) { ModelState.AddModelError(Campo(ex), Mensaje(ex)); return View(modelo); }
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var producto = repositorio.Buscar(id);
        return producto == null ? View("NoEncontrado") : View(new ProductoFormularioViewModel
        { Id = producto.Id, Nombre = producto.Nombre, Precio = producto.Precio, Cantidad = producto.Cantidad });
    }

    [HttpPost]
    public IActionResult Editar(int id, ProductoFormularioViewModel modelo)
    {
        if (repositorio.Buscar(id) == null) return View("NoEncontrado");
        modelo.Id = id;
        Validar(modelo);
        if (!ModelState.IsValid) return View(modelo);
        try
        {
            if (!repositorio.Actualizar(id, modelo.Nombre!, modelo.Precio!.Value, modelo.Cantidad!.Value))
                return View("NoEncontrado");
            TempData["Exito"] = "Producto actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex) { ModelState.AddModelError(Campo(ex), Mensaje(ex)); return View(modelo); }
    }

    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var producto = repositorio.Buscar(id);
        return producto == null ? View("NoEncontrado") : View(producto);
    }

    [HttpPost, ActionName("Eliminar")]
    public IActionResult ConfirmarEliminar(int id)
    {
        if (!repositorio.Eliminar(id)) return View("NoEncontrado");
        TempData["Exito"] = "Producto eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error() => View();

    private void Validar(ProductoFormularioViewModel modelo)
    {
        if (modelo.Nombre != null)
        {
            try { ValidadorProducto.ValidarNombre(modelo.Nombre); }
            catch (ArgumentException ex) { ModelState.AddModelError(nameof(modelo.Nombre), Mensaje(ex)); }
        }
        if (modelo.Precio.HasValue)
        {
            try { ValidadorProducto.ValidarPrecio(modelo.Precio.Value); }
            catch (ArgumentOutOfRangeException ex) { ModelState.AddModelError(nameof(modelo.Precio), Mensaje(ex)); }
            if (decimal.Round(modelo.Precio.Value, 2) != modelo.Precio.Value)
                ModelState.AddModelError(nameof(modelo.Precio), "El precio admite como máximo dos decimales.");
        }
        if (modelo.Cantidad.HasValue)
        {
            try { ValidadorProducto.ValidarCantidad(modelo.Cantidad.Value); }
            catch (ArgumentOutOfRangeException ex) { ModelState.AddModelError(nameof(modelo.Cantidad), Mensaje(ex)); }
        }
    }

    private static string Campo(ArgumentException ex) => ex.ParamName switch
    {
        "nombre" => "Nombre",
        "precio" => "Precio",
        "cantidad" => "Cantidad",
        _ => ""
    };

    private static string Mensaje(ArgumentException ex) => ex.Message.Split('\n')[0].Trim();
}
