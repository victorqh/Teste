# ?? Guía de Configuración de Base de Datos PostgreSQL

## ? Opción AUTOMÁTICA (Recomendada)

Entity Framework Core creará automáticamente la base de datos. Solo sigue estos pasos:

### Paso 1: Instalar PostgreSQL
Si no tienes PostgreSQL instalado:
- Descarga desde: https://www.postgresql.org/download/
- Instala con la configuración por defecto
- Anota tu contraseña de usuario `postgres`

### Paso 2: Configurar Contraseña
Edita `appsettings.json` y cambia la contraseña si es diferente:
```json
"DefaultConnection": "Host=localhost;Database=appFinalDb;Username=postgres;Password=TU_PASSWORD_AQUI"
```

### Paso 3: Instalar Herramienta EF Core
Abre una terminal en Visual Studio (Tools > Command Line > Developer PowerShell) y ejecuta:
```bash
dotnet tool install --global dotnet-ef
```

### Paso 4: Crear Migración
En la misma terminal, ejecuta:
```bash
dotnet ef migrations add InitialCreate
```
Esto creará una carpeta `Migrations` con el código para crear la base de datos.

### Paso 5: Aplicar Migración (Crear Base de Datos)
Ejecuta:
```bash
dotnet ef database update
```

? **¡LISTO!** La base de datos `appFinalDb` ha sido creada automáticamente con todas las tablas de Identity.

---

## ?? Opción MANUAL (Si prefieres crear la BD manualmente)

### Paso 1: Conectar a PostgreSQL
Abre pgAdmin o ejecuta en terminal:
```bash
psql -U postgres
```

### Paso 2: Crear Base de Datos
```sql
CREATE DATABASE "appFinalDb";
```

### Paso 3: Ejecutar Migraciones
Luego ejecuta los comandos de Entity Framework mencionados arriba.

---

## ?? Verificar que la Base de Datos Existe

### Opción 1: Con pgAdmin
1. Abre pgAdmin
2. Conecta al servidor local
3. Verás la base de datos `appFinalDb` en la lista

### Opción 2: Con Terminal
```bash
psql -U postgres -d appFinalDb -c "\dt"
```
Esto mostrará las tablas creadas (AspNetUsers, AspNetRoles, etc.)

---

## ?? Ejecutar la Aplicación

1. Presiona F5 en Visual Studio
2. La aplicación se abrirá en tu navegador
3. Ve a la página de Registro y crea un usuario
4. ¡Listo! Ahora puedes iniciar sesión

---

## ?? Tablas Creadas Automáticamente

Entity Framework Identity crea estas tablas:
- **AspNetUsers** - Usuarios registrados
- **AspNetRoles** - Roles del sistema
- **AspNetUserRoles** - Relación usuario-rol
- **AspNetUserClaims** - Claims de usuarios
- **AspNetUserLogins** - Logins externos (Google, Facebook)
- **AspNetUserTokens** - Tokens de autenticación
- **AspNetRoleClaims** - Claims de roles

---

## ?? Solución de Problemas

### Error: "Cannot connect to PostgreSQL"
- Verifica que PostgreSQL esté corriendo (busca el servicio en Windows)
- Comprueba la contraseña en `appsettings.json`

### Error: "Password authentication failed"
- Cambia la contraseña en `appsettings.json` por la correcta

### Error: "dotnet ef not found"
- Ejecuta: `dotnet tool install --global dotnet-ef`
- Cierra y abre la terminal nuevamente

---

## ?? Comandos Útiles de EF Core

```bash
# Ver migraciones aplicadas
dotnet ef migrations list

# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir última migración
dotnet ef database update NombreMigracionAnterior

# Eliminar última migración (si no se aplicó)
dotnet ef migrations remove

# Ver script SQL que se ejecutará
dotnet ef migrations script
```

---

## ?? Resumen: ¿Qué hace EF Core automáticamente?

1. ? Crea la base de datos `appFinalDb` si no existe
2. ? Crea todas las tablas necesarias para Identity
3. ? Configura las relaciones entre tablas
4. ? Crea índices y constraints
5. ? Mantiene un historial de cambios en la carpeta `Migrations`

**No necesitas escribir SQL manualmente** ?
