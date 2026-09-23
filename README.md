# Gestión de Productos

Proyecto académico en C# y .NET 9 con interfaz web ASP.NET Core MVC, vistas Razor, Bootstrap 5 y SQLite. Incluye cinco pruebas unitarias manuales de una función de negocio aislada. No usa xUnit, NUnit ni MSTest.

La aplicación incluye una pantalla de inventario con totales de productos y unidades, valor monetario de existencias, alertas de stock bajo o agotado, búsqueda, filtros por estado y accesos para consultar o actualizar cada producto.

## Estructura

```text
GestionProductos.sln
src/GestionProductos.Core/              Modelo y validaciones independientes
src/GestionProductos.Data/              SQLite y consultas parametrizadas
src/GestionProductos.Web/               Controlador, ViewModels, vistas, CSS y JS
tests/GestionProductos.PruebasManuales/  Cinco pruebas con if y try-catch
productos.db                             Base local, excluida de Git
verificar-web.ps1                        Comprobación HTTP con base temporal
GUION_VIDEO.md                           Guion para dos integrantes
```

## Ejecutar

Se requiere el SDK de .NET 9. Desde la raíz de este proyecto, en PowerShell:

```powershell
dotnet restore GestionProductos.sln --configfile NuGet.Config
dotnet build GestionProductos.sln -c Release --no-restore
dotnet run --project src/GestionProductos.Web -c Release --no-build --no-restore
```

Abra **http://localhost:5168** en el navegador. En Visual Studio, haga clic derecho en `GestionProductos.Web` en el Explorador de soluciones, elija **Establecer como proyecto de inicio** y presione F5. Si Visual Studio ya tenía abierta la solución, vuelva a cargarla para actualizar los proyectos visibles.

Las pruebas manuales se ejecutan por separado:

```powershell
dotnet run --project tests/GestionProductos.PruebasManuales -c Release --no-build --no-restore
```

## Base de datos

La web usa una carpeta de datos de Windows con permisos de escritura estables:

```text
C:\Users\USUARIO\AppData\Local\GestionProductos\productos.db
```

Al iniciar por primera vez, si existe una base de una versión anterior en la raíz de la solución, se copia automáticamente a esta ubicación sin sobrescribir una base ya migrada. Así se conservan los registros y SQLite puede crear sus archivos auxiliares al ejecutar desde Visual Studio. El archivo `.db` está excluido de Git. En una instalación nueva, la web crea automáticamente el archivo, la carpeta y la tabla `Productos`. Para usar otra ruta, configure la variable de entorno `Database__Path` o `Database:Path` en `src/GestionProductos.Web/appsettings.json`. La web no borra registros al arrancar.

El precio se recibe como `decimal`, se limita a dos decimales, se multiplica por 100 y se guarda como entero en `PrecioCentavos`. Por ejemplo, `Q12.25` se guarda como `1225`. Al consultar se divide por `100m`. SQLite exige nombre no vacío, precio positivo y cantidad no negativa. Las consultas con valores usan parámetros SQL. Los formularios POST tienen protección antifalsificación y validación en servidor.

## Pruebas aisladas

`GestionProductos.PruebasManuales` referencia **solo** `GestionProductos.Core`. No referencia Data ni Web, no inicia el servidor y no abre SQLite. Llama directamente a `ValidadorProducto.ValidarPrecio(decimal)`. La misma función se usa en el flujo web de creación y actualización antes de guardar.

| Caso | Entrada | Esperado |
|---|---:|---|
| 1 | `100m` | Sin excepción |
| 2 | `0.01m` | Sin excepción |
| 3 | `0m` | `ArgumentOutOfRangeException` |
| 4 | `-25m` | `ArgumentOutOfRangeException` |
| 5 | `-0.01m` | `ArgumentOutOfRangeException` |

Cada caso muestra entrada, esperado, obtenido y estado. Una excepción de otro tipo falla la prueba. El proceso devuelve 0 si todas pasan y 1 si alguna falla. **Un dato inválido rechazado correctamente representa una prueba exitosa.**

## Verificación

Ejecute `./verificar-web.ps1` para comprobar el CRUD por HTTP con una base temporal independiente. El script revisa estado vacío, creación válida e inválida, listado, búsqueda, detalle, edición válida e inválida, persistencia al reiniciar el servidor, confirmación de eliminación y estado vacío final.

Resultados realizados en este entorno:

- Restauración: correcta.
- Compilación Release de la solución: 0 errores, 0 advertencias.
- Pruebas manuales aisladas: 5 pasaron, 0 fallaron; código 0.
- Verificación HTTP: pasó con base SQLite temporal.
- Inicio de la web con la base principal conservada: respondió correctamente.
- Revisión visual en navegador de escritorio: inicio y formulario correctos. La vista móvil aún no se comprobó visualmente.

## Entrega

Repositorio actualizado: <https://github.com/Edu28933/GestionProductos>

Falta grabar y entregar el videotutorial de máximo 10 minutos siguiendo `GUION_VIDEO.md`. El archivo local `productos.db` y las carpetas `bin/` y `obj/` están excluidos del repositorio.
