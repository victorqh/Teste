using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using appUPN.Repositories;

namespace appUPN.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder
    public class CarritoController : Controller
    {
        private readonly ICarritoRepository _carritoRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly ILogger<CarritoController> _logger;

        public CarritoController(
            ICarritoRepository carritoRepository, 
            IProductoRepository productoRepository,
            ILogger<CarritoController> logger)
        {
            _carritoRepository = carritoRepository;
            _productoRepository = productoRepository;
            _logger = logger;
        }

        // Obtener UserId del usuario logueado
        private int ObtenerUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

        // GET: Carrito
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = ObtenerUserId();
                var carrito = await _carritoRepository.ObtenerCarritoPorUserIdAsync(userId);

                if (carrito == null)
                {
                    ViewBag.Items = new List<Models.CarritoItem>();
                    ViewBag.Total = 0m;
                    ViewBag.CantidadItems = 0;
                }
                else
                {
                    var items = await _carritoRepository.ObtenerItemsDelCarritoAsync(carrito.CarritoId);
                    var total = await _carritoRepository.ObtenerTotalCarritoAsync(carrito.CarritoId);
                    var cantidad = await _carritoRepository.ObtenerCantidadItemsAsync(carrito.CarritoId);

                    ViewBag.Items = items;
                    ViewBag.Total = total;
                    ViewBag.CantidadItems = cantidad;
                }

                ViewBag.UserName = User.Identity?.Name;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el carrito");
                TempData["ErrorMessage"] = "Error al cargar el carrito";
                return View();
            }
        }

        // POST: Carrito/Agregar
        [HttpPost]
        public async Task<IActionResult> Agregar(int productoId, int cantidad = 1)
        {
            try
            {
                var userId = ObtenerUserId();
                
                // Validar producto
                var producto = await _productoRepository.ObtenerPorIdAsync(productoId);
                if (producto == null || !producto.EstaActivo)
                {
                    TempData["ErrorMessage"] = "Producto no disponible";
                    return RedirectToAction("Index", "Productos");
                }

                // Validar stock
                if (producto.Stock < cantidad)
                {
                    TempData["ErrorMessage"] = $"Stock insuficiente. Solo quedan {producto.Stock} unidades";
                    return RedirectToAction("Detalle", "Productos", new { id = productoId });
                }

                // Obtener o crear carrito
                var carrito = await _carritoRepository.ObtenerCarritoPorUserIdAsync(userId);
                if (carrito == null)
                {
                    carrito = await _carritoRepository.CrearCarritoAsync(userId);
                }

                // Verificar si el producto ya está en el carrito
                var itemExistente = await _carritoRepository.ObtenerItemAsync(carrito.CarritoId, productoId);

                if (itemExistente != null)
                {
                    // Actualizar cantidad
                    var nuevaCantidad = itemExistente.Cantidad + cantidad;
                    
                    if (producto.Stock < nuevaCantidad)
                    {
                        TempData["ErrorMessage"] = $"Stock insuficiente. Solo quedan {producto.Stock} unidades";
                        return RedirectToAction(nameof(Index));
                    }

                    itemExistente.Cantidad = nuevaCantidad;
                    await _carritoRepository.ActualizarItemAsync(itemExistente);
                }
                else
                {
                    // Agregar nuevo item
                    var nuevoItem = new Models.CarritoItem
                    {
                        CarritoId = carrito.CarritoId,
                        ProductoId = productoId,
                        Cantidad = cantidad,
                        Precio = producto.Precio
                    };

                    await _carritoRepository.AgregarItemAsync(nuevoItem);
                }

                _logger.LogInformation("Usuario {UserId} agregó producto {ProductoId} al carrito", userId, productoId);
                TempData["SuccessMessage"] = "Producto agregado al carrito";
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto al carrito");
                TempData["ErrorMessage"] = "Error al agregar producto";
                return RedirectToAction("Index", "Productos");
            }
        }

        // POST: Carrito/ActualizarCantidad
        [HttpPost]
        public async Task<IActionResult> ActualizarCantidad(int itemId, int cantidad)
        {
            try
            {
                if (cantidad <= 0)
                {
                    return await Eliminar(itemId);
                }

                var userId = ObtenerUserId();
                var carrito = await _carritoRepository.ObtenerCarritoPorUserIdAsync(userId);
                
                if (carrito == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var items = await _carritoRepository.ObtenerItemsDelCarritoAsync(carrito.CarritoId);
                var item = items.FirstOrDefault(i => i.CarritoItemId == itemId);

                if (item == null || item.Producto == null)
                {
                    TempData["ErrorMessage"] = "Item no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                // Validar stock
                if (item.Producto.Stock < cantidad)
                {
                    TempData["ErrorMessage"] = $"Stock insuficiente. Solo quedan {item.Producto.Stock} unidades";
                    return RedirectToAction(nameof(Index));
                }

                item.Cantidad = cantidad;
                await _carritoRepository.ActualizarItemAsync(item);

                TempData["SuccessMessage"] = "Cantidad actualizada";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cantidad");
                TempData["ErrorMessage"] = "Error al actualizar cantidad";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Carrito/Eliminar
        [HttpPost]
        public async Task<IActionResult> Eliminar(int itemId)
        {
            try
            {
                await _carritoRepository.EliminarItemAsync(itemId);
                
                TempData["SuccessMessage"] = "Producto eliminado del carrito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar item del carrito");
                TempData["ErrorMessage"] = "Error al eliminar producto";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
