using System.Diagnostics;
using appUPN.Models;
using appUPN.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace appUPN.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductoRepository productoRepository, ILogger<HomeController> logger)
        {
            _productoRepository = productoRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtener productos destacados (ofertas)
                var productosDestacados = await _productoRepository.ObtenerOfertasDestacadasAsync(6);
                return View(productosDestacados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la página principal");
                return View(new List<Models.Producto>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
