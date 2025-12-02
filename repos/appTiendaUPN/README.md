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

## 🔄 Flujo de Consultas SQL

El proyecto implementa el patrón **Repository + Service + Controller** para separar responsabilidades:

```
VISTA → CONTROLLER → SERVICE → REPOSITORY → ENTITY FRAMEWORK CORE → POSTGRESQL
```

### Ejemplo: Obtener productos activos

1. **Vista** (`Productos/Index.cshtml`): Usuario accede a la página de productos
2. **Controller** (`ProductosController.cs`):
   ```csharp
   public async Task<IActionResult> Index(int? categoriaId)
   {
       productos = await _productoService.GetProductosActivosAsync();
       return View(productos);
   }
   ```

3. **Service** (`ProductoService.cs`):
   ```csharp
   public async Task<IEnumerable<Producto>> GetProductosActivosAsync()
   {
       return await _productoRepository.GetActivosAsync();
   }
   ```

4. **Repository** (`ProductoRepository.cs`):
   ```csharp
   public async Task<IEnumerable<Producto>> GetActivosAsync()
   {
       return await _context.Productos
           .Include(p => p.Categoria)  // JOIN
           .Where(p => p.EstaActivo && p.Stock > 0)
           .OrderByDescending(p => p.FechaCreacion)
           .ToListAsync();
   }
   ```

5. **Entity Framework Core** traduce el LINQ a SQL:
   ```sql
   SELECT p.*, c.*
   FROM productos p
   INNER JOIN categorias c ON p.categoriaid = c.categoriaid
   WHERE p.estaactivo = true AND p.stock > 0
   ORDER BY p.fechacreacion DESC;
   ```

6. **PostgreSQL** ejecuta la consulta y devuelve los resultados

### Ventajas del patrón

- ✅ **Separación de responsabilidades**: Cada capa tiene un propósito específico
- ✅ **Testeable**: Puedes hacer mocks de repositorios para pruebas
- ✅ **Mantenible**: Cambios en la BD no afectan al Controller
- ✅ **Reutilizable**: Múltiples controllers pueden usar el mismo service

### Inyección de Dependencias

En `Program.cs` se registran las interfaces y sus implementaciones:
```csharp
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
builder.Services.AddScoped<ICarritoService, CarritoService>();
```

**AddScoped** crea una instancia nueva por cada petición HTTP.

## 🛡️ Autorización con [Authorize]

El atributo `[Authorize]` protege controllers o actions para que solo usuarios autenticados puedan acceder.

### Implementación en el proyecto

En `CarritoController.cs`:
```csharp
[Authorize]  // Protege TODO el controller
public class CarritoController : Controller
{
    // Todos estos métodos requieren login:
    public async Task<IActionResult> Index() { }
    public async Task<IActionResult> Agregar(int productoId) { }
    public async Task<IActionResult> Eliminar(int itemId) { }
}
```

### Flujo de autorización

1. **Usuario no autenticado** intenta acceder a `/Carrito`
2. **Middleware de autorización** detecta que falta autenticación
3. **Redirección automática** a `/Account/Login?ReturnUrl=/Carrito`
4. **Usuario inicia sesión** correctamente
5. **Redirección de vuelta** a `/Carrito` (la URL original)

### Configuración en Program.cs

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });
```

### Uso en vistas

En las vistas Razor puedes verificar autenticación:
```csharp
@if (User.Identity!.IsAuthenticated)
{
    <span>Hola, @User.Identity.Name</span>
    <a href="/Account/Logout">Cerrar Sesión</a>
}
else
{
    <a href="/Account/Login">Iniciar Sesión</a>
}
```

### Variantes del atributo

- `[Authorize]` - Requiere cualquier usuario autenticado
- `[Authorize(Roles = "Admin")]` - Solo usuarios con rol Admin
- `[AllowAnonymous]` - Permite acceso sin autenticación (excepción en controller protegido)

## 🎨 Interfaz

- Diseño responsive con **Bootstrap 5**
- Formularios con validaciones en tiempo real
- Mensajes de error y éxito
- Navbar dinámico según estado de autenticación



Framework principal: ASP.NET Core MVC (C#)
Base de datos: PostgreSQL
ORM: Entity Framework Core
Dependencias principales:
Npgsql.EntityFrameworkCore.PostgreSQL (conexión a PostgreSQL)
BCrypt.Net-Next (hash de contraseñas)
Front-end: Razor Views (.cshtml) con Bootstrap 5 y Bootstrap Icons
Seguridad:
Autenticación por cookies
Contraseñas hasheadas con BCrypt
Validaciones en formularios
Protección contra CSRF
Arquitectura:
Patrón MVC
Repositories y Services para separar lógica de negocio y acceso a datos
Funcionalidades:
Registro y login de usuarios
Catálogo de productos, ofertas, búsqueda
Carrito de compras
Navbar dinámico según autenticación
No se usó PHP ni HTML puro; todo el front está en Razor y Bootstrap.



La seguridad por cookies en ASP.NET Core funciona así:

Cuando un usuario inicia sesión correctamente, el servidor crea una cookie de autenticación y la envía al navegador.
Esta cookie contiene información cifrada sobre la identidad del usuario (no la contraseña), y se almacena en el navegador.
En cada petición siguiente, el navegador envía la cookie al servidor.
El servidor valida la cookie y, si es válida, reconoce al usuario como autenticado.
Si la cookie no existe o no es válida, el usuario no tiene acceso a las páginas protegidas.
Las cookies pueden expirar, y se pueden invalidar al cerrar sesión. Todo el proceso es seguro si usas HTTPS y no guardas información sensible directamente en la cookie.


Cuando un usuario inicia sesión en la página de Login, el controlador valida el usuario y la contraseña.
Si son correctos, el servidor crea una cookie de autenticación y la envía al navegador del usuario.
En cada visita a una página protegida, el navegador envía esa cookie al servidor.
El servidor verifica la cookie y, si es válida, permite el acceso a las páginas protegidas (por ejemplo, el carrito, las ofertas, etc.).
Si la cookie no está presente o no es válida (por ejemplo, si el usuario no ha iniciado sesión o la cookie expiró), el usuario es redirigido a la página de Login o a AccessDenied.
Así, tu web usa cookies para recordar qué usuario está autenticado y proteger las páginas privadas. Todo esto se maneja automáticamente por ASP.NET Core y el middleware de autenticación por cookies.