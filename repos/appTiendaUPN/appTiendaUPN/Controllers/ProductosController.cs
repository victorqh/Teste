using Microsoft.AspNetCore.Mvc;
using appTiendaUPN.Services;

namespace appTiendaUPN.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly RecomendacionService _recomendacionService;

        public ProductosController(IProductoService productoService, RecomendacionService recomendacionService)
        {
            _productoService = productoService;
            _recomendacionService = recomendacionService;
        }

        // GET: Productos
        public async Task<IActionResult> Index(int? categoriaId)
        {
            IEnumerable<appTiendaUPN.Models.Producto> productos;
            
            if (categoriaId.HasValue && categoriaId.Value > 0)
            {
                productos = await _productoService.GetProductosPorCategoriaAsync(categoriaId.Value);
                ViewData["CategoriaSeleccionada"] = categoriaId.Value;
            }
            else
            {
                productos = await _productoService.GetProductosActivosAsync();
            }
            
            return View(productos);
        }

        // GET: Productos/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            
            if (producto == null)
            {
                return NotFound();
            }

            // -- config para ia: Obtener recomendaciones
            var recomendaciones = await _recomendacionService.RecomendarAsync(producto);
            ViewData["Recomendaciones"] = recomendaciones;

            return View(producto);
        }

        // GET: Productos/Ofertas
        public async Task<IActionResult> Ofertas()
        {
            var ofertas = await _productoService.GetOfertasAsync();
            return View(ofertas);
        }

        // GET: Productos/Buscar?q=laptop
        public async Task<IActionResult> Buscar(string q)
        {
            ViewData["SearchTerm"] = q;
            var productos = await _productoService.BuscarProductosAsync(q ?? "");
            return View(productos);
        }
    }
}
