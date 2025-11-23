# ?? COMANDOS PARA EJECUTAR AHORA

## ? YA CAMBIÉ TU CONFIGURACIÓN A:
```json
"Password=postgres"
```

---

## ?? PRUEBA 1: Ejecutar migración con la contraseña común

Abre la **Package Manager Console** en Visual Studio y ejecuta:

```powershell
dotnet ef database update
```

### ¿Funcionó? ?
? ¡Perfecto! La base de datos ya está creada.

### ¿Sigue fallando? ?
? Continúa con Prueba 2

---

## ?? PRUEBA 2: Verificar conexión directa a PostgreSQL

En la **terminal de Visual Studio**, ejecuta:

```powershell
psql -U postgres -h localhost -c "SELECT version();"
```

### Te pide contraseña:
Prueba estas en orden:
1. `postgres`
2. `admin`
3. `root`
4. `1234`

### ¿Alguna funcionó? ?
? Actualiza `appsettings.json` con esa contraseña y vuelve a ejecutar:
```powershell
dotnet ef database update
```

### Ninguna funciona ?
? Continúa con Prueba 3

---

## ??? PRUEBA 3: Abrir pgAdmin y verificar

1. Busca **pgAdmin 4** en tu computadora
2. Ábrelo
3. Click en "Servers" ? "PostgreSQL [versión]"
4. Te pedirá contraseña
5. Prueba contraseñas hasta que conecte
6. La que funcione, úsala en `appsettings.json`

---

## ?? PRUEBA 4: Resetear contraseña de PostgreSQL

### Paso 1: Editar pg_hba.conf
Abre como **Administrador** el archivo:
```
C:\Program Files\PostgreSQL\17\data\pg_hba.conf
```
(O la versión que tengas instalada)

### Paso 2: Buscar y cambiar esta línea
**ANTES:**
```
host    all             all             127.0.0.1/32            scram-sha-256
```

**DESPUÉS (temporal):**
```
host    all             all             127.0.0.1/32            trust
```

### Paso 3: Reiniciar PostgreSQL
Ejecuta en PowerShell como Administrador:
```powershell
Restart-Service postgresql-x64-17
```
(Cambia "17" por tu versión)

O manualmente:
1. `Win + R`
2. Escribe: `services.msc`
3. Busca: `postgresql-x64-17`
4. Click derecho ? Reiniciar

### Paso 4: Cambiar la contraseña
Abre Command Prompt:
```cmd
psql -U postgres
```

Dentro de PostgreSQL:
```sql
ALTER USER postgres WITH PASSWORD 'miNuevaContraseña';
\q
```

### Paso 5: Revertir pg_hba.conf
Vuelve a cambiar la línea a:
```
host    all             all             127.0.0.1/32            scram-sha-256
```

### Paso 6: Reiniciar PostgreSQL de nuevo
```powershell
Restart-Service postgresql-x64-17
```

### Paso 7: Actualizar appsettings.json
```json
"Password=miNuevaContraseña"
```

### Paso 8: Probar
```powershell
cd appFinal
dotnet ef database update
```

---

## ?? CHECKLIST DE VERIFICACIÓN:

Antes de ejecutar las migraciones, verifica:

- [ ] PostgreSQL está corriendo (services.msc)
- [ ] El puerto es 5432 (o el que uses)
- [ ] El usuario es "postgres"
- [ ] La contraseña es correcta
- [ ] appsettings.json está guardado
- [ ] Estás en la carpeta del proyecto

---

## ?? SECUENCIA DE COMANDOS COMPLETA:

Una vez que tengas la contraseña correcta:

```powershell
# Navegar al proyecto
cd C:\Users\UPN-VQH\source\repos\appFinal\appFinal

# Limpiar migraciones anteriores (si existen)
dotnet ef migrations remove --force

# Crear nueva migración
dotnet ef migrations add InitialCreate

# Aplicar migración (crear base de datos)
dotnet ef database update

# Verificar que se creó
psql -U postgres -d appFinalDb -c "\dt"
```

---

## ? SEÑALES DE ÉXITO:

Cuando funcione, verás:
```
Build started...
Build succeeded.
Applying migration '20250602_InitialCreate'.
Done.
```

Y en pgAdmin verás:
- Base de datos: `appFinalDb`
- Tablas:
  - AspNetUsers
  - AspNetRoles
  - AspNetUserRoles
  - AspNetUserClaims
  - AspNetRoleClaims
  - AspNetUserLogins
  - AspNetUserTokens

---

## ?? SI NADA FUNCIONA:

### Opción Nuclear: Reinstalar PostgreSQL

1. Desinstala PostgreSQL completamente
2. Descarga la versión más reciente: https://www.postgresql.org/download/windows/
3. Durante la instalación:
   - **Anota la contraseña que establezcas**
   - Puerto: 5432
   - Locale: Spanish, Spain
   - Instala pgAdmin
4. Una vez instalado, actualiza `appsettings.json`
5. Ejecuta las migraciones

---

## ?? TRUCO RÁPIDO:

Si tienes pgAdmin abierto y funciona, entonces la contraseña que usaste ahí es la correcta. Simplemente usa esa misma en tu código.

---

## ?? ESTADO ACTUAL:

- ? Código completado
- ? Paquetes instalados
- ? Migraciones creadas
- ? Base de datos NO creada (problema de contraseña)
- ?? appsettings.json actualizado a `Password=postgres`

**Siguiente paso**: Ejecuta `dotnet ef database update` y verifica si funciona con la nueva contraseña.
