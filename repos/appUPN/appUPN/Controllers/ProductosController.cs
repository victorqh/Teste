using Microsoft.AspNetCore.Mvc;
using appUPN.Repositories;

namespace appUPN.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductoRepository productoRepository, ILogger<ProductosController> logger)
        {
            _productoRepository = productoRepository;
            _logger = logger;
        }

        // GET: Productos
        public async Task<IActionResult> Index(int? categoriaId)
        {
            try
            {
                var productos = categoriaId.HasValue
                    ? await _productoRepository.ObtenerPorCategoriaAsync(categoriaId.Value)
                    : await _productoRepository.ObtenerTodosAsync();

                ViewBag.Categorias = await _productoRepository.ObtenerCategoriasAsync();
                ViewBag.CategoriaSeleccionada = categoriaId;

                return View(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar productos");
                ViewBag.Categorias = new List<Models.Categoria>();
                return View(new List<Models.Producto>());
            }
        }

        // GET: Productos/Detalle/5
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var producto = await _productoRepository.ObtenerPorIdAsync(id.Value);

                if (producto == null)
                {
                    return NotFound();
                }

                return View(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar detalle del producto {ProductoId}", id);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Productos/Ofertas
        public async Task<IActionResult> Ofertas()
        {
            try
            {
                var productos = await _productoRepository.ObtenerOfertasAsync();

                ViewBag.Categorias = await _productoRepository.ObtenerCategoriasAsync();
                ViewBag.CategoriaSeleccionada = null;

                return View("Index", productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar ofertas");
                ViewBag.Categorias = new List<Models.Categoria>();
                return View("Index", new List<Models.Producto>());
            }
        }
    }
}
