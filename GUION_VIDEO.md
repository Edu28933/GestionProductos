# Guion completo para el videotutorial

## Objetivo y duración

Este guion está preparado para **dos integrantes** y una duración aproximada de **8 minutos con 40 segundos**. No debe superar los 10 minutos.

El video debe demostrar tres cosas:

1. Que existe un CRUD funcional de productos.
2. Que `ValidarPrecio` está aislada de la interfaz web y de SQLite.
3. Que las pruebas manuales comparan resultados válidos e inválidos mediante condicionales y `try-catch`.

No es necesario modificar ni separar código durante la grabación. El aislamiento ya está realizado mediante los proyectos `GestionProductos.Core`, `GestionProductos.Data`, `GestionProductos.Web` y `GestionProductos.PruebasManuales`. En el video deben **mostrar y explicar esa separación**.

---

## Parte 1: preparación antes de grabar

Estos pasos se hacen antes de iniciar la grabación para evitar perder tiempo.

### 1. Abrir la carpeta del proyecto

La carpeta es:

```text
C:\Users\eduar\Documents\ChatGPT\proyecto para ASEGURAMIENTO DE LA CALIDAD DE SOFTWARE
```

### 2. Abrir Visual Studio

1. Abra Visual Studio.
2. Seleccione **Abrir un proyecto o una solución**.
3. Abra el archivo:

```text
C:\Users\eduar\Documents\ChatGPT\proyecto para ASEGURAMIENTO DE LA CALIDAD DE SOFTWARE\GestionProductos.sln
```

4. En el Explorador de soluciones confirme que aparecen estos proyectos:
   - `GestionProductos.Core`
   - `GestionProductos.Data`
   - `GestionProductos.Web`
   - `GestionProductos.PruebasManuales`
5. Haga clic derecho en `GestionProductos.Web`.
6. Seleccione **Establecer como proyecto de inicio**.

### 3. Preparar los archivos que se mostrarán

Abra estas pestañas en Visual Studio y déjelas listas:

1. `src/GestionProductos.Core/Validaciones/ValidadorProducto.cs`
2. `tests/GestionProductos.PruebasManuales/Program.cs`
3. `tests/GestionProductos.PruebasManuales/GestionProductos.PruebasManuales.csproj`
4. `src/GestionProductos.Web/Controllers/ProductosController.cs`
5. `src/GestionProductos.Data/ProductoRepository.cs`

En `ValidadorProducto.cs`, deje visible este método:

```csharp
public static void ValidarPrecio(decimal precio)
{
    if (precio <= 0)
        throw new ArgumentOutOfRangeException(
            nameof(precio),
            "El precio debe ser mayor que cero");
}
```

### 4. Preparar PowerShell

Abra PowerShell y ejecute:

```powershell
cd "C:\Users\eduar\Documents\ChatGPT\proyecto para ASEGURAMIENTO DE LA CALIDAD DE SOFTWARE"
```

Deje preparado este comando, pero todavía no lo ejecute:

```powershell
dotnet run --project "tests\GestionProductos.PruebasManuales\GestionProductos.PruebasManuales.csproj" -c Release
```

### 5. Preparar el CRUD

1. En Visual Studio presione **F5** o el botón verde.
2. Espere a que abra el navegador en `http://localhost:5168`.
3. Compruebe que la página carga sin errores.
4. Si ya existen productos, puede usarlos. También puede crear un producto de demostración durante el video.
5. Use estos datos para que ambos integrantes sepan qué escribir:

```text
Nombre: Cuaderno universitario
Precio: 25.50
Cantidad: 10
```

Para demostrar una validación inválida use:

```text
Precio: 0
```

### 6. Preparar la grabación

1. Comprueben que ambos micrófonos se escuchan claramente.
2. Cierren ventanas personales y notificaciones.
3. Aumenten el tamaño de letra de Visual Studio y PowerShell si es necesario.
4. No muestren contraseñas, correos privados ni credenciales.
5. Tengan abierto el repositorio de GitHub o GitLab antes de la parte final.

---

## Parte 2: grabación completa

## 0:00–0:40 — Presentación

**Habla:** Integrante 1  
**Pantalla:** Página principal del CRUD en el navegador.

### Acción en pantalla

Muestre la página completa: barra lateral, tarjetas, buscador y tabla de productos.

### Texto sugerido

> “Buenos días. Somos [nombre del integrante 1] y [nombre del integrante 2]. Nuestro proyecto se llama Gestión de Productos. Es un CRUD desarrollado en C# con ASP.NET Core MVC y una base de datos SQLite. En esta demostración mostraremos brevemente el funcionamiento del sistema y después explicaremos cómo aislamos y probamos manualmente la función crítica que valida el precio.”

> “El objetivo principal es verificar valores válidos e inválidos sin que la prueba dependa de la página web, de la base de datos ni de servicios externos.”

### Idea que debe quedar clara

La evaluación se centra principalmente en la prueba aislada. El CRUD sirve como contexto real donde se utiliza la función.

---

## 0:40–1:10 — Explicar la página principal

**Habla:** Integrante 1  
**Pantalla:** Página principal.

### Acción en pantalla

Señale visualmente:

- Total de productos.
- Total de unidades.
- Valor de existencias.
- Buscador.
- Botón **Nuevo producto**.
- Acciones **Ver**, **Editar** y **Eliminar**.

### Texto sugerido

> “Esta es la pantalla principal del inventario. Las tarjetas muestran datos reales obtenidos de SQLite: la cantidad de productos, la suma de unidades y el valor total de las existencias. En la tabla podemos buscar productos por nombre o identificador y acceder a las operaciones de consulta, edición y eliminación.”

---

## 1:10–2:05 — Registrar un producto válido

**Habla:** Integrante 1  
**Pantalla:** Formulario **Nuevo producto**.

### Acción en pantalla

1. Haga clic en **Nuevo producto**.
2. Complete:
   - Nombre: `Cuaderno universitario`
   - Precio: `25.50`
   - Cantidad: `10`
3. Haga clic en **Guardar producto**.
4. Muestre el mensaje verde de confirmación.
5. Señale el producto agregado en la tabla.

### Texto sugerido

> “Registraremos un producto válido. El nombre es obligatorio, el precio debe ser mayor que cero y acepta un máximo de dos decimales, y la cantidad debe ser cero o mayor.”

> “Al guardar, el servidor vuelve a validar los datos antes de enviarlos al repositorio. El producto aparece en la tabla y los valores de las tarjetas se actualizan usando los datos reales.”

---

## 2:05–2:45 — Demostrar una entrada inválida

**Habla:** Integrante 1  
**Pantalla:** Formulario para editar el producto.

### Acción en pantalla

1. Haga clic en **Editar** sobre el producto creado.
2. Cambie únicamente el precio a `0`.
3. Intente guardar.
4. Muestre el mensaje: **El precio debe ser mayor que cero**.
5. Presione **Cancelar** o vuelva al listado.
6. Muestre que el precio anterior sigue siendo `Q25.50`.

### Texto sugerido

> “Ahora usamos un precio inválido igual a cero. La aplicación muestra el mensaje junto al campo y no modifica el registro.”

> “Esta validación del navegador ayuda al usuario, pero la regla también se ejecuta obligatoriamente en el servidor. Por eso un dato inválido no se guarda aunque se intente enviar directamente el formulario.”

### Idea que debe quedar clara

Una edición inválida conserva el dato anterior en SQLite.

---

## 2:45–3:20 — Mostrar detalle y eliminación segura

**Habla:** Integrante 1  
**Pantalla:** Listado, detalle y confirmación de eliminación.

### Acción en pantalla

1. Haga clic en **Ver** y muestre el detalle.
2. Regrese al listado.
3. Haga clic en **Eliminar**.
4. Muestre la pantalla de confirmación con el nombre del producto.
5. Primero haga clic en **Cancelar**.
6. Vuelva a abrir **Eliminar**.
7. Confirme con **Sí, eliminar**.
8. Muestre el mensaje de éxito.

### Texto sugerido

> “La vista de detalle presenta la información completa. Para eliminar no se borra desde una petición de consulta. Primero se muestra una confirmación y la eliminación solamente se realiza después de enviar el formulario mediante POST.”

> “Cancelar conserva el registro. Al confirmar, el producto se elimina y la página muestra un mensaje de éxito.”

---

## 3:20–4:05 — Explicar la estructura del proyecto

**Habla:** Integrante 1  
**Pantalla:** Explorador de soluciones de Visual Studio.

### Acción en pantalla

Expanda los cuatro proyectos y señálelos uno por uno.

### Texto sugerido

> “La solución está separada en cuatro proyectos. GestionProductos.Web contiene la interfaz, el controlador, los formularios y las vistas. GestionProductos.Data contiene la conexión SQLite y las consultas parametrizadas. GestionProductos.Core contiene el modelo y las reglas de negocio. Finalmente, GestionProductos.PruebasManuales es un ejecutable independiente que prueba directamente la validación.”

> “Esta separación permite probar la unidad más pequeña sin iniciar el sitio web y sin abrir la base de datos.”

### Esquema que pueden decir

```text
Web → Data → Core
PruebasManuales → Core
```

La flecha representa una referencia entre proyectos. Las pruebas no tienen flechas hacia Web ni Data.

---

## 4:05–4:45 — Mostrar dónde usa el CRUD la validación

**Habla:** Integrante 1  
**Pantalla:** `ProductosController.cs`, método `Validar`.

### Acción en pantalla

Muestre la llamada:

```csharp
ValidadorProducto.ValidarPrecio(modelo.Precio.Value);
```

Después muestre brevemente `ProductoRepository.cs`, donde también se valida antes de convertir a centavos.

### Texto sugerido

> “En el controlador, tanto la creación como la actualización llaman al método de validación antes de persistir. Las excepciones esperadas se convierten en mensajes comprensibles junto al campo.”

> “El repositorio también reutiliza la misma regla antes de convertir el precio a centavos. No copiamos la comparación precio mayor que cero en distintas partes del sistema; la regla central está en Core.”

> “El precio se maneja como decimal en C# y se almacena como entero en centavos. Por ejemplo, 25.50 se guarda como 2550.”

Aquí termina aproximadamente la participación del integrante 1.

---

## 4:45–5:35 — Explicar la función aislada

**Habla:** Integrante 2  
**Pantalla:** `ValidadorProducto.cs`.

### Acción en pantalla

Resalte únicamente `ValidarPrecio`.

### Texto sugerido

> “La unidad que elegimos probar es ValidarPrecio. Es un método público y estático que recibe solamente un decimal. Si el valor es mayor que cero termina normalmente. Si es cero o negativo lanza ArgumentOutOfRangeException con el mensaje ‘El precio debe ser mayor que cero’.”

> “Decimos que está aislada porque no recibe un repositorio ni una conexión. No consulta SQLite, no abre archivos, no solicita información al usuario, no imprime resultados, no inicia la web y no llama APIs. Su resultado depende únicamente del valor recibido.”

> “La función está dentro de Core. Core no tiene referencia al proyecto Web ni al proyecto Data. Así evitamos que la lógica de negocio dependa de la infraestructura.”

### Definición sencilla de aislamiento

Pueden memorizar esta frase:

> “Aislar significa ejecutar solamente la regla que queremos verificar, controlando directamente su entrada y observando su salida o excepción, sin involucrar componentes externos.”

---

## 5:35–6:15 — Demostrar que el proyecto de pruebas solo referencia Core

**Habla:** Integrante 2  
**Pantalla:** `GestionProductos.PruebasManuales.csproj`.

### Acción en pantalla

Muestre esta referencia:

```xml
<ProjectReference Include="../../src/GestionProductos.Core/GestionProductos.Core.csproj" />
```

### Texto sugerido

> “Esta es la evidencia estructural del aislamiento. El proyecto de pruebas tiene una sola referencia: GestionProductos.Core. No referencia GestionProductos.Data, no instala Microsoft.Data.Sqlite y tampoco referencia GestionProductos.Web.”

> “Por lo tanto, cuando ejecutamos estas pruebas no se inicia el servidor ni se abre el archivo productos.db.”

### Qué no deben decir

No digan que la prueba usa una base temporal. La base temporal corresponde al script funcional del CRUD. Las cinco pruebas de `ValidarPrecio` no usan ninguna base.

---

## 6:15–7:20 — Explicar los cinco casos y las aserciones

**Habla:** Integrante 2  
**Pantalla:** `tests/GestionProductos.PruebasManuales/Program.cs`.

### Acción en pantalla

Primero muestre el arreglo `casos`. Después desplácese al bloque `try-catch`.

### Texto sugerido para los casos

> “Diseñamos cinco casos. Los valores 100 y 0.01 son válidos y esperamos que no lancen ninguna excepción. Los valores cero, menos 25 y menos 0.01 son inválidos y esperamos exactamente ArgumentOutOfRangeException.”

> “Usamos 0.01 porque representa el precio positivo mínimo permitido con dos decimales. También probamos cero, un negativo común y un negativo pequeño para cubrir claramente el límite.”

### Texto sugerido para el `try-catch`

> “La llamada a ValidarPrecio se encuentra dentro del try. Si no ocurre una excepción, el caso pasa solamente cuando esperábamos un valor válido.”

> “El primer catch reconoce específicamente ArgumentOutOfRangeException. Ese caso pasa solamente cuando esperábamos rechazar el dato. Un segundo catch captura cualquier otra excepción y marca la prueba como fallida, porque no aceptamos cualquier error como resultado correcto.”

> “Después, un condicional incrementa el número de pruebas aprobadas y se imprime el nombre del caso, la entrada, el resultado esperado, el resultado obtenido y el estado PASÓ o FALLÓ.”

### Frase esencial

> “Un dato inválido rechazado correctamente representa una prueba exitosa.”

### Cómo explicar un caso de fallo

> “La prueba fallaría si un precio cero no lanzara ninguna excepción, si un precio válido lanzara una excepción o si apareciera un tipo de excepción diferente al esperado.”

No deben alterar el código para provocar un fallo durante el video. Basta con explicar cuándo fallaría.

---

## 7:20–8:10 — Ejecutar las pruebas en vivo

**Habla:** Integrante 2  
**Pantalla:** PowerShell.

### Acción en pantalla

Compruebe que PowerShell está en la carpeta correcta. El prompt debe terminar aproximadamente así:

```text
...\proyecto para ASEGURAMIENTO DE LA CALIDAD DE SOFTWARE>
```

Ejecute:

```powershell
dotnet run --project "tests\GestionProductos.PruebasManuales\GestionProductos.PruebasManuales.csproj" -c Release
```

Espere el resultado y señale:

```text
Total: 5 | Pasaron: 5 | Fallaron: 0
```

### Texto sugerido

> “Ahora ejecutamos el proyecto de pruebas manuales de forma independiente. Podemos observar la entrada, lo esperado y lo obtenido en cada caso.”

> “Los dos precios válidos terminaron sin excepción. Los tres inválidos produjeron exactamente ArgumentOutOfRangeException. El resumen indica cinco pruebas ejecutadas, cinco aprobadas y cero fallidas.”

> “El programa devuelve código de salida cero porque todas las pruebas coinciden con el resultado esperado.”

### Si aparece el error “no es un archivo de proyecto válido”

Significa que PowerShell está en otra carpeta. Ejecute:

```powershell
cd "C:\Users\eduar\Documents\ChatGPT\proyecto para ASEGURAMIENTO DE LA CALIDAD DE SOFTWARE"
```

Después repita el comando de las pruebas.

---

## 8:10–8:40 — Repositorio, conclusión y despedida

**Habla:** Integrante 2  
**Pantalla:** Repositorio de GitHub o GitLab.

### Acción en pantalla

1. Muestre la URL real del repositorio.
2. Muestre brevemente las carpetas `src`, `tests`, el `README.md` y este guion.
3. No muestre una URL inventada o de ejemplo.

### Texto sugerido

> “En el repositorio actualizado se encuentran el CRUD web, la capa de datos, la biblioteca Core, las pruebas manuales y la documentación.”

> “Con esto demostramos que la regla de precio se usa en un CRUD real y que también puede verificarse como una unidad pequeña y totalmente aislada. Gracias.”

---

## Parte 3: lista de control final

Antes de entregar, confirmen todo lo siguiente:

- [ ] El video dura menos de 10 minutos.
- [ ] Participan y hablan ambos integrantes.
- [ ] Se muestra brevemente el CRUD funcionando.
- [ ] Se registra un producto válido.
- [ ] Se demuestra el rechazo del precio `0`.
- [ ] Se explica que el registro anterior se conserva si la validación falla.
- [ ] Se muestra `ValidadorProducto.ValidarPrecio`.
- [ ] Se explica por qué la función está aislada.
- [ ] Se muestra que PruebasManuales referencia solamente Core.
- [ ] Se explican las entradas `100`, `0.01`, `0`, `-25` y `-0.01`.
- [ ] Se explica el `try-catch` y los condicionales.
- [ ] Se ejecutan las pruebas en vivo.
- [ ] La terminal muestra `5 pasaron` y `0 fallaron`.
- [ ] Se explica que rechazar correctamente un dato inválido hace pasar la prueba.
- [ ] Se muestra la URL real del repositorio.
- [ ] El repositorio contiene el código actualizado.
- [ ] Se entregan tanto el enlace del video como el enlace del repositorio.

---

## Respuestas rápidas por si el docente pregunta

### ¿Qué unidad probaron?

> “El método estático `ValidarPrecio(decimal precio)` de la biblioteca Core.”

### ¿Por qué es una prueba unitaria?

> “Porque ejecuta directamente una única regla de negocio y controla su entrada y resultado sin usar la base de datos, el servidor web ni otras dependencias.”

### ¿Cómo saben que no usa SQLite?

> “El proyecto de pruebas solo referencia Core. Core no depende de Data y el método no contiene conexión, repositorio ni operaciones de archivos.”

### ¿Por qué un precio negativo produce PASÓ?

> “Porque el resultado esperado era que la regla lo rechazara con `ArgumentOutOfRangeException`. La prueba aprueba cuando el resultado real coincide con el esperado.”

### ¿Cuándo fallaría la prueba?

> “Fallaría si un valor inválido fuera aceptado, si un valor válido fuera rechazado o si se lanzara una excepción diferente.”

### ¿Por qué guardan el precio en centavos?

> “Para almacenar de forma consistente los dos decimales como un entero. Por ejemplo, Q25.50 se guarda como 2550 centavos.”

### ¿Las validaciones HTML son suficientes?

> “No. También validamos obligatoriamente en el servidor mediante la función de Core antes de guardar.”

### ¿Dónde está la base de datos?

> “Está en `productos.db`, en la raíz de la solución. Las pruebas manuales no abren ese archivo.”
