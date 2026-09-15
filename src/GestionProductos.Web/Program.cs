using System.Globalization;
using GestionProductos.Data;
using Microsoft.AspNetCore.Mvc;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

// La base se guarda en una carpeta de usuario con permisos de escritura estables.
var raiz = BuscarRaiz(builder.Environment.ContentRootPath) ?? BuscarRaiz(AppContext.BaseDirectory)
    ?? throw new InvalidOperationException("No se encontró GestionProductos.sln para resolver la base de datos.");
var configurada = builder.Configuration["Database:Path"];
var ruta = !string.IsNullOrWhiteSpace(configurada)
    ? Path.GetFullPath(Path.IsPathRooted(configurada) ? configurada : Path.Combine(raiz, configurada))
    : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "GestionProductos", "productos.db");

// Migra una base creada por versiones anteriores sin sobrescribir una base ya migrada.
var rutaAnterior = Path.Combine(raiz, "productos.db");
Directory.CreateDirectory(Path.GetDirectoryName(ruta)!);
if (string.IsNullOrWhiteSpace(configurada) && File.Exists(rutaAnterior) && !File.Exists(ruta))
    File.Copy(rutaAnterior, ruta);

var baseDatos = new BaseDatos(ruta);
baseDatos.Inicializar();
builder.Services.AddSingleton(baseDatos);
builder.Services.AddSingleton<ProductoRepository>();

var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Productos/Error");
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(name: "default", pattern: "{controller=Productos}/{action=Index}/{id?}");
app.Run();

static string? BuscarRaiz(string desde)
{
    for (var directorio = new DirectoryInfo(desde); directorio != null; directorio = directorio.Parent)
        if (File.Exists(Path.Combine(directorio.FullName, "GestionProductos.sln"))) return directorio.FullName;
    return null;
}
