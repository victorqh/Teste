using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace appUPN.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder
    public class CarritoController : Controller
    {
        private readonly ILogger<CarritoController> _logger;

        public CarritoController(ILogger<CarritoController> logger)
        {
            _logger = logger;
        }

        // GET: Carrito
        public IActionResult Index()
        {
            // Obtener el ID del usuario actual
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userName = User.Identity?.Name;

            ViewBag.UserName = userName;
            ViewBag.UserId = userId;

            // Aquí irá la lógica del carrito
            return View();
        }

        // POST: Carrito/Agregar
        [HttpPost]
        public IActionResult Agregar(int productoId, int cantidad = 1)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
                // Aquí agregarías la lógica para agregar al carrito
                _logger.LogInformation("Usuario {UserId} agregó producto {ProductoId}", userId, productoId);

                TempData["SuccessMessage"] = "Producto agregado al carrito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto al carrito");
                TempData["ErrorMessage"] = "Error al agregar producto";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
