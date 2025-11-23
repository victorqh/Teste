# Sistema de Autenticación - appTiendaUPN

## ✅ Sistema Completado

Se ha implementado un sistema completo de autenticación con login y registro.

## 📋 Características Implementadas

- ✅ **Registro de usuarios** con validaciones
- ✅ **Login** con email y contraseña
- ✅ **Hash seguro de contraseñas** usando BCrypt
- ✅ **Autenticación por cookies**
- ✅ **Sesión persistente** (opción "Recordarme")
- ✅ **Botones dinámicos** en el navbar (Login/Registro o Usuario/Logout)
- ✅ **Protección de rutas** (AccessDenied)
- ✅ **Integración con PostgreSQL** (base de datos `appFinal`)

## 🚀 Cómo Usar

### 1. Asegúrate que PostgreSQL esté corriendo

La aplicación se conecta a:
- **Host:** localhost
- **Puerto:** 5432
- **Base de datos:** appFinal
- **Usuario:** postgres
- **Contraseña:** admin

### 2. Verifica que la tabla Users existe en tu base de datos

La tabla ya debe estar creada con el script que proporcionaste:
```sql
CREATE TABLE Users (
    UserId SERIAL PRIMARY KEY,
    Nombre VARCHAR(120) NOT NULL,
    Email VARCHAR(120) UNIQUE NOT NULL,
    PasswordHash VARCHAR(500) NOT NULL,
    Telefono VARCHAR(20),
    Direccion VARCHAR(200),
    Rol VARCHAR(20) NOT NULL DEFAULT 'Cliente',
    FechaRegistro TIMESTAMP NOT NULL DEFAULT NOW()
);
```

### 3. Ejecuta la aplicación

```powershell
dotnet run
```

### 4. Prueba el sistema

1. **Ve a la aplicación** (generalmente https://localhost:5001)
2. **Haz clic en "Registrarse"** en el navbar
3. **Completa el formulario** de registro
4. **Inicia sesión** con las credenciales creadas
5. **Verás tu nombre** en el navbar cuando estés autenticado

## 📁 Archivos Creados/Modificados

### Modelos
- `Models/User.cs` - Entidad de usuario
- `Models/LoginViewModel.cs` - ViewModel para login
- `Models/RegisterViewModel.cs` - ViewModel para registro

### Controlador
- `Controllers/AccountController.cs` - Maneja login, registro y logout

### Vistas
- `Views/Account/Login.cshtml` - Página de inicio de sesión
- `Views/Account/Register.cshtml` - Página de registro
- `Views/Account/AccessDenied.cshtml` - Página de acceso denegado

### Configuración
- `Program.cs` - Configuración de autenticación
- `appsettings.json` - Cadena de conexión a PostgreSQL
- `Data/ApplicationDbContext.cs` - DbContext con DbSet<User>
- `Views/Shared/_Layout.cshtml` - Navbar con botones de login/logout

## 🔐 Seguridad

- Las contraseñas se hashean con **BCrypt** antes de guardarlas
- Autenticación basada en **cookies seguras**
- Validaciones tanto en cliente como servidor
- Protección contra **CSRF** con tokens antiforgery

## 👤 Roles

Por defecto, todos los usuarios se registran con el rol "Cliente". Puedes extender esto para agregar más roles (Admin, etc.) en el futuro.

## 🎨 Interfaz

- Diseño responsive con **Bootstrap 5**
- Formularios con validaciones en tiempo real
- Mensajes de error y éxito
- Navbar dinámico según estado de autenticación
