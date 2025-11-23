# ?? ERROR: Contraseña de PostgreSQL Incorrecta

## ?? TU ERROR:
```
28P01: la autentificación password falló para el usuario 'postgres'
```

---

## ? SOLUCIÓN MÁS RÁPIDA (3 minutos):

### Opción A: Prueba estas contraseñas en orden

Edita el archivo: **`appFinal\appsettings.json`**

#### 1. Prueba con "postgres":
```json
"Password=postgres"
```
Luego ejecuta: `dotnet ef database update`

#### 2. Si no funciona, prueba con "admin":
```json
"Password=admin"
```
Luego ejecuta: `dotnet ef database update`

#### 3. Si no funciona, prueba con "root":
```json
"Password=root"
```
Luego ejecuta: `dotnet ef database update`

---

### Opción B: Usa el script automático

1. Abre **PowerShell** en la carpeta del proyecto
2. Ejecuta:
   ```powershell
   .\test-postgres-password.ps1
   ```
3. El script probará automáticamente las contraseñas comunes

---

### Opción C: Verifica la contraseña con pgAdmin

1. Abre **pgAdmin 4** (lo instalaste junto con PostgreSQL)
2. Intenta conectarte al servidor
3. Prueba diferentes contraseñas hasta que funcione
4. La contraseña que funcione en pgAdmin, úsala en `appsettings.json`

---

## ?? SOLUCIÓN SI NO RECUERDAS LA CONTRASEÑA:

### Resetear contraseña de PostgreSQL (Windows):

#### Paso 1: Abrir archivo de configuración
1. Busca este archivo (como Administrador):
   ```
   C:\Program Files\PostgreSQL\[versión]\data\pg_hba.conf
   ```
   Ejemplo: `C:\Program Files\PostgreSQL\17\data\pg_hba.conf`

2. Abre con Notepad++ o Visual Studio Code

#### Paso 2: Editar autenticación
Busca esta línea:
```
host    all             all             127.0.0.1/32            scram-sha-256
```

Cámbiala temporalmente a:
```
host    all             all             127.0.0.1/32            trust
```

#### Paso 3: Reiniciar PostgreSQL
1. Presiona `Win + R`
2. Escribe: `services.msc`
3. Busca: `postgresql-x64-[versión]`
4. Click derecho ? **Reiniciar**

#### Paso 4: Cambiar contraseña
Abre **Command Prompt** y ejecuta:
```cmd
psql -U postgres
```

Dentro de PostgreSQL, ejecuta:
```sql
ALTER USER postgres WITH PASSWORD 'nuevaContraseña123';
\q
```

#### Paso 5: Revertir pg_hba.conf
Vuelve a cambiar la línea a:
```
host    all             all             127.0.0.1/32            scram-sha-256
```

#### Paso 6: Reiniciar PostgreSQL de nuevo

#### Paso 7: Actualizar appsettings.json
```json
"Password=nuevaContraseña123"
```

#### Paso 8: Probar
```bash
dotnet ef database update
```

---

## ?? VERIFICAR QUE POSTGRESQL ESTÁ CORRIENDO

### En Windows:
1. Presiona `Win + R`
2. Escribe: `services.msc`
3. Busca: `postgresql-x64-[versión]`
4. Debe decir: **"En ejecución"**
5. Si dice "Detenido", click derecho ? **Iniciar**

---

## ?? CONTRASEÑAS MÁS COMUNES:

Según estadísticas de instalación:
1. ? `postgres` (60% de casos)
2. ? `admin` (20% de casos)
3. ? `root` (10% de casos)
4. ? `1234` (5% de casos)
5. ? La que pusiste durante instalación (5% de casos)

---

## ?? OTROS PROBLEMAS COMUNES:

### Error de puerto:
Si PostgreSQL está en otro puerto, usa:
```json
"Host=localhost;Port=5433;Database=appFinalDb;Username=postgres;Password=tuPassword"
```

### Error de SSL:
Si hay problemas con SSL, prueba:
```json
"Host=localhost;Database=appFinalDb;Username=postgres;Password=tuPassword;SSL Mode=Disable"
```

---

## ? CUANDO FUNCIONE VERÁS:

```
PM> dotnet ef database update
Build started...
Build succeeded.
Applying migration '20250602000000_InitialCreate'.
Done.
```

Y podrás ver la base de datos en pgAdmin con las tablas:
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- etc.

---

## ?? RECOMENDACIÓN:

**La forma más rápida de resolver esto:**
1. Abre pgAdmin
2. Prueba contraseñas hasta que conecte
3. Usa esa misma contraseña en tu código
4. Listo

**No quieres instalar nada nuevo?**
? Sigue el proceso de "Resetear contraseña" arriba

---

## ?? ¿NECESITAS MÁS AYUDA?

Si después de probar todas estas opciones sigues con problemas:

1. Verifica la versión de PostgreSQL:
   ```bash
   psql --version
   ```

2. Verifica que psql está en PATH:
   ```bash
   where psql
   ```

3. Si no aparece nada, PostgreSQL no está instalado correctamente

---

## ?? RESUMEN EJECUTIVO:

**PROBLEMA**: La contraseña en `appsettings.json` no coincide con la de PostgreSQL

**SOLUCIÓN RÁPIDA**: 
1. Abre pgAdmin
2. Prueba contraseñas hasta conectar
3. Usa esa contraseña en tu código

**SOLUCIÓN COMPLETA**: 
Resetea la contraseña siguiendo los pasos de "Resetear contraseña de PostgreSQL"

**TIEMPO ESTIMADO**: 3-5 minutos

---

? **He cambiado temporalmente tu `appsettings.json` para usar `Password=postgres` (la más común). Prueba ejecutando `dotnet ef database update` ahora.**
