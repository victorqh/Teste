using Microsoft.AspNetCore.Mvc;
using appTiendaUPN.Services;

namespace appTiendaUPN.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.GetProductosActivosAsync();
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
