# ?? INICIO RÁPIDO - Ejecuta estos comandos

## ?? Los paquetes ya están instalados ?

## ??? Crear la Base de Datos

### Paso 1: Instalar herramienta EF Core (solo una vez)
Abre la terminal en Visual Studio y ejecuta:
```bash
dotnet tool install --global dotnet-ef
```

### Paso 2: Navegar a la carpeta del proyecto
```bash
cd appFinal
```

### Paso 3: Crear la migración inicial
```bash
dotnet ef migrations add InitialCreate
```

### Paso 4: Crear la base de datos
```bash
dotnet ef database update
```

## ? ¡LISTO!

Tu aplicación ya está lista. Presiona **F5** para ejecutarla.

---

## ?? Credenciales de PostgreSQL por Defecto

Si configuraste PostgreSQL con las opciones por defecto:
- **Host**: localhost
- **Puerto**: 5432
- **Usuario**: postgres
- **Contraseña**: La que configuraste durante la instalación
- **Base de Datos**: appFinalDb (se crea automáticamente)

**IMPORTANTE**: Si tu contraseña de PostgreSQL es diferente a "postgres", edita el archivo `appsettings.json` y cambia la línea:
```json
"DefaultConnection": "Host=localhost;Database=appFinalDb;Username=postgres;Password=TU_PASSWORD_AQUI"
```

---

## ?? Funcionalidades Implementadas

? Página principal con menú dinámico  
? Sistema de registro de usuarios  
? Sistema de login  
? Sistema de logout  
? Autenticación con ASP.NET Identity  
? Conexión a PostgreSQL  
? Validación de formularios  
? Diseño responsive con Bootstrap  

---

## ?? Probar la Aplicación

1. Ejecuta la aplicación (F5)
2. Haz clic en "Registrarse"
3. Completa el formulario:
   - Nombre completo
   - Email
   - Contraseña (mínimo 6 caracteres, con mayúscula y número)
4. Después del registro, serás redirigido a la página principal
5. ¡Ya puedes usar la aplicación!

---

## ?? Estructura del Proyecto

```
appFinal/
??? Data/
?   ??? ApplicationDbContext.cs     ? Contexto de base de datos
?   ??? ApplicationUser.cs          ? Modelo de usuario
??? Pages/
?   ??? Account/
?   ?   ??? Login.cshtml           ? Página de login
?   ?   ??? Login.cshtml.cs
?   ?   ??? Register.cshtml        ? Página de registro
?   ?   ??? Register.cshtml.cs
?   ?   ??? Logout.cshtml.cs       ? Lógica de logout
?   ??? Shared/
?   ?   ??? _Layout.cshtml         ? Layout principal
?   ?   ??? _ValidationScriptsPartial.cshtml
?   ??? Index.cshtml               ? Página de inicio
?   ??? Index.cshtml.cs
?   ??? _ViewImports.cshtml
?   ??? _ViewStart.cshtml
??? appsettings.json               ? Configuración (incluye conexión DB)
??? appsettings.Development.json
??? Program.cs                     ? Punto de entrada de la aplicación
```

---

## ?? Solución Rápida de Errores

### "Cannot connect to database"
? Verifica que PostgreSQL esté corriendo (servicios de Windows)

### "Password authentication failed"
? Cambia la contraseña en `appsettings.json`

### "dotnet-ef: command not found"
? Ejecuta: `dotnet tool install --global dotnet-ef`  
? Cierra y abre la terminal nuevamente

### Errores de compilación
? Ejecuta: `dotnet clean` y luego `dotnet build`

---

## ?? Siguiente Paso

Una vez que la base de datos esté creada y la aplicación funcione, puedes:
- Agregar más campos al usuario
- Crear roles (Admin, Usuario, etc.)
- Agregar más páginas protegidas
- Implementar funcionalidades de negocio

¡Tu base de código está lista para crecer! ??
