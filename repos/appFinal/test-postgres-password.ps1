# Script para probar diferentes contraseñas de PostgreSQL
# Ejecuta este script en PowerShell

Write-Host "=== Probando conexión a PostgreSQL ===" -ForegroundColor Cyan

$passwords = @("postgres", "admin", "root", "1234", "password", "12345678", "")

foreach ($pwd in $passwords) {
    Write-Host "`nProbando contraseña: '$pwd'" -ForegroundColor Yellow
    
    $env:PGPASSWORD = $pwd
    
    try {
        $result = psql -U postgres -h localhost -c "SELECT version();" 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "? ¡CONTRASEÑA CORRECTA ENCONTRADA: '$pwd'!" -ForegroundColor Green
            Write-Host "`nActualiza tu appsettings.json con:" -ForegroundColor Cyan
            Write-Host "Password=$pwd" -ForegroundColor White
            
            # Actualizar automáticamente appsettings.json
            $appSettingsPath = "appFinal\appsettings.json"
            if (Test-Path $appSettingsPath) {
                Write-Host "`n¿Quieres actualizar appsettings.json automáticamente? (S/N)" -ForegroundColor Yellow
                $response = Read-Host
                
                if ($response -eq "S" -or $response -eq "s") {
                    $content = Get-Content $appSettingsPath -Raw
                    $newContent = $content -replace 'Password=[^;"]*', "Password=$pwd"
                    Set-Content $appSettingsPath $newContent
                    Write-Host "? appsettings.json actualizado!" -ForegroundColor Green
                }
            }
            
            break
        }
    }
    catch {
        Write-Host "? Contraseña incorrecta" -ForegroundColor Red
    }
}

Write-Host "`n=== Fin de la prueba ===" -ForegroundColor Cyan
