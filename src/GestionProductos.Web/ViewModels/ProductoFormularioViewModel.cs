using System.ComponentModel.DataAnnotations;

namespace GestionProductos.Web.ViewModels;

public sealed class ProductoFormularioViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingrese el nombre.")]
    [Display(Name = "Nombre")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "Ingrese el precio.")]
    [Display(Name = "Precio")]
    public decimal? Precio { get; set; }

    [Required(ErrorMessage = "Ingrese la cantidad.")]
    [Display(Name = "Cantidad")]
    public int? Cantidad { get; set; }
}
