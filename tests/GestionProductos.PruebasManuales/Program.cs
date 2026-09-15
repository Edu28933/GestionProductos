using GestionProductos.Core.Validaciones;

var casos = new (string Nombre, decimal Entrada, bool DebeLanzar)[]
{
    ("Caso 1: precio normal", 100m, false),
    ("Caso 2: mínimo válido", 0.01m, false),
    ("Caso 3: cero", 0m, true),
    ("Caso 4: negativo", -25m, true),
    ("Caso 5: negativo pequeño", -0.01m, true)
};

int pasadas = 0;
foreach (var caso in casos)
{
    string esperado = caso.DebeLanzar ? "ArgumentOutOfRangeException" : "sin excepción";
    string obtenido;
    bool paso;
    try
    {
        ValidadorProducto.ValidarPrecio(caso.Entrada);
        obtenido = "sin excepción";
        paso = !caso.DebeLanzar;
    }
    catch (ArgumentOutOfRangeException)
    {
        obtenido = "ArgumentOutOfRangeException";
        paso = caso.DebeLanzar;
    }
    catch (Exception ex)
    {
        obtenido = ex.GetType().Name;
        paso = false;
    }
    if (paso) pasadas++;
    Console.WriteLine($"{caso.Nombre} | Entrada: {caso.Entrada} | Esperado: {esperado} | Obtenido: {obtenido} | Estado: {(paso ? "PASÓ" : "FALLÓ")}");
}

// Un dato inválido rechazado correctamente representa una prueba exitosa.
Console.WriteLine($"Total: {casos.Length} | Pasaron: {pasadas} | Fallaron: {casos.Length - pasadas}");
return pasadas == casos.Length ? 0 : 1;
