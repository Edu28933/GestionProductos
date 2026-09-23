$ErrorActionPreference = 'Stop'
$db = Join-Path $env:TEMP ("gestion-web-" + [guid]::NewGuid().ToString('N') + '.db')
$web = Join-Path $PSScriptRoot 'src/GestionProductos.Web'
$url = 'http://127.0.0.1:5179'
$env:Database__Path = $db
$proceso = $null
try {
    $proceso = Start-Process -FilePath 'dotnet' -ArgumentList @('run','--project',"`"$web`"",'-c','Release','--no-build','--no-restore','--urls',$url) -WorkingDirectory $web -WindowStyle Hidden -PassThru
    $sesion = New-Object Microsoft.PowerShell.Commands.WebRequestSession
    $inicio = $null
    for ($i = 0; $i -lt 40; $i++) {
        try { $inicio = Invoke-WebRequest "$url/Productos" -WebSession $sesion; break } catch { Start-Sleep -Milliseconds 250 }
    }
    if (-not $inicio -or $inicio.StatusCode -ne 200 -or -not [System.Net.WebUtility]::HtmlDecode($inicio.Content).Contains('Aún no hay productos')) { throw 'Estado vacío no disponible' }
    $crear = Invoke-WebRequest "$url/Productos/Crear" -WebSession $sesion
    $token = [regex]::Match($crear.Content, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"').Groups[1].Value
    if (-not $token) { throw 'Falta token antifalsificación' }
    $invalidaCrear = Invoke-WebRequest "$url/Productos/Crear" -Method Post -WebSession $sesion -Body @{Nombre='No guardar';Precio='0';Cantidad='1';__RequestVerificationToken=$token}
    if (-not [System.Net.WebUtility]::HtmlDecode($invalidaCrear.Content).Contains('El precio debe ser mayor que cero')) { throw 'No se rechazó el registro inválido' }
    $sinRegistro = Invoke-WebRequest "$url/Productos" -WebSession $sesion
    if (-not [System.Net.WebUtility]::HtmlDecode($sinRegistro.Content).Contains('Aún no hay productos')) { throw 'El registro inválido alteró los datos' }
    $respuesta = Invoke-WebRequest "$url/Productos/Crear" -Method Post -WebSession $sesion -Body @{Nombre='Cuaderno';Precio='12.50';Cantidad='3';__RequestVerificationToken=$token}
    if (-not $respuesta.Content.Contains('Producto guardado correctamente')) { throw 'No se creó el producto' }
    $lista = Invoke-WebRequest "$url/Productos?busqueda=Cuaderno" -WebSession $sesion
    if (-not $lista.Content.Contains('Cuaderno') -or -not $lista.Content.Contains('Q37.50')) { throw 'Listado, búsqueda o resumen incorrecto' }
    $inventario = Invoke-WebRequest "$url/Productos/Inventario" -WebSession $sesion
    $inventarioHtml = [System.Net.WebUtility]::HtmlDecode($inventario.Content)
    foreach ($texto in @('CONTROL DE EXISTENCIAS', 'Valor total', 'Stock bajo', 'Cuaderno', 'Q37.50')) {
        if (-not $inventarioHtml.Contains($texto)) { throw "Inventario incompleto: $texto" }
    }
    $inventarioFiltrado = Invoke-WebRequest "$url/Productos/Inventario?estado=bajo" -WebSession $sesion
    if (-not $inventarioFiltrado.Content.Contains('1 resultado(s)') -or -not $inventarioFiltrado.Content.Contains('Cuaderno')) { throw 'Filtro de inventario incorrecto' }
    $detalle = Invoke-WebRequest "$url/Productos/Detalle/1" -WebSession $sesion
    if (-not $detalle.Content.Contains('Cuaderno')) { throw 'Detalle incorrecto' }
    $editar = Invoke-WebRequest "$url/Productos/Editar/1" -WebSession $sesion
    $token = [regex]::Match($editar.Content, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"').Groups[1].Value
    $invalida = Invoke-WebRequest "$url/Productos/Editar/1" -Method Post -WebSession $sesion -Body @{Nombre='Mal';Precio='0';Cantidad='3';__RequestVerificationToken=$token}
    if (-not [System.Net.WebUtility]::HtmlDecode($invalida.Content).Contains('El precio debe ser mayor que cero')) { throw 'No se mostró validación de precio' }
    $detalle = Invoke-WebRequest "$url/Productos/Detalle/1" -WebSession $sesion
    if (-not $detalle.Content.Contains('Cuaderno')) { throw 'La edición inválida cambió los datos' }
    $respuesta = Invoke-WebRequest "$url/Productos/Editar/1" -Method Post -WebSession $sesion -Body @{Nombre='Agenda';Precio='9.25';Cantidad='4';__RequestVerificationToken=$token}
    if (-not $respuesta.Content.Contains('Producto actualizado correctamente')) { throw 'No se actualizó' }
    Stop-Process -Id $proceso.Id -Force
    Start-Sleep -Milliseconds 400
    $proceso = Start-Process -FilePath 'dotnet' -ArgumentList @('run','--project',"`"$web`"",'-c','Release','--no-build','--no-restore','--urls',$url) -WorkingDirectory $web -WindowStyle Hidden -PassThru
    $persistido = $null
    for ($i = 0; $i -lt 40; $i++) {
        try { $persistido = Invoke-WebRequest "$url/Productos/Detalle/1" -WebSession $sesion; break } catch { Start-Sleep -Milliseconds 250 }
    }
    if (-not $persistido -or -not $persistido.Content.Contains('Agenda')) { throw 'No persistió al reiniciar el servidor' }
    $eliminar = Invoke-WebRequest "$url/Productos/Eliminar/1" -WebSession $sesion
    if (-not $eliminar.Content.Contains('Agenda')) { throw 'No se mostró confirmación' }
    $detalle = Invoke-WebRequest "$url/Productos/Detalle/1" -WebSession $sesion
    if (-not $detalle.Content.Contains('Agenda')) { throw 'Cancelar eliminación alteró el registro' }
    $token = [regex]::Match($eliminar.Content, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"').Groups[1].Value
    $respuesta = Invoke-WebRequest "$url/Productos/Eliminar/1" -Method Post -WebSession $sesion -Body @{__RequestVerificationToken=$token}
    if (-not $respuesta.Content.Contains('Producto eliminado correctamente') -or -not [System.Net.WebUtility]::HtmlDecode($respuesta.Content).Contains('Aún no hay productos')) { throw 'Eliminación o estado vacío incorrecto' }
    Write-Output 'WEB: CRUD, validaciones, persistencia y pantalla de inventario: PASÓ'
}
finally {
    if ($proceso) { Stop-Process -Id $proceso.Id -Force -ErrorAction SilentlyContinue }
    Remove-Item Env:Database__Path -ErrorAction SilentlyContinue
    if (Test-Path -LiteralPath $db) { Remove-Item -LiteralPath $db }
}
