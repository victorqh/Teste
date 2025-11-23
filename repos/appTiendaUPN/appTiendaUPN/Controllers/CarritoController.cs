using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using appTiendaUPN.Services;

namespace appTiendaUPN.Controllers
{
    [Authorize]
    public class CarritoController : Controller
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

        // GET: Carrito
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var carrito = await _carritoService.GetOrCreateCarritoAsync(userId);
            return View(carrito);
        }

        // POST: Carrito/Agregar
        [HttpPost]
        public async Task<IActionResult> Agregar(int productoId, int cantidad = 1)
        {
            var userId = GetUserId();
            var resultado = await _carritoService.AgregarProductoAsync(userId, productoId, cantidad);

            if (resultado)
            {
                TempData["Success"] = "Producto agregado al carrito";
            }
            else
            {
                TempData["Error"] = "No se pudo agregar el producto al carrito";
            }

            return RedirectToAction("Index", "Productos");
        }

        // POST: Carrito/Actualizar
        [HttpPost]
        public async Task<IActionResult> Actualizar(int productoId, int cantidad)
        {
            var userId = GetUserId();
            await _carritoService.ActualizarCantidadAsync(userId, productoId, cantidad);
            return RedirectToAction(nameof(Index));
        }

        // POST: Carrito/Remover
        [HttpPost]
        public async Task<IActionResult> Remover(int carritoItemId)
        {
            var userId = GetUserId();
            await _carritoService.RemoverProductoAsync(userId, carritoItemId);
            return RedirectToAction(nameof(Index));
        }

        // POST: Carrito/Vaciar
        [HttpPost]
        public async Task<IActionResult> Vaciar()
        {
            var userId = GetUserId();
            await _carritoService.VaciarCarritoAsync(userId);
            return RedirectToAction(nameof(Index));
        }

        // GET: API para obtener cantidad de items (para el badge del navbar)
        [HttpGet]
        public async Task<IActionResult> GetCantidad()
        {
            var userId = GetUserId();
            var cantidad = await _carritoService.GetCantidadItemsAsync(userId);
            return Json(new { cantidad });
        }
    }
}
