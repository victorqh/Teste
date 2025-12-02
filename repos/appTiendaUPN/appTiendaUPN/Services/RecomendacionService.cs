// -- config para ia: Servicio de recomendaciones simples
using appTiendaUPN.Models;
using appTiendaUPN.Repositories;

namespace appTiendaUPN.Services
{
    public class RecomendacionService
    {
        private readonly IProductoRepository _productoRepository;

        public RecomendacionService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        // Recomienda productos relacionados por categoría o tipo
        public async Task<IEnumerable<Producto>> RecomendarAsync(Producto producto, int max = 4)
        {
            // Ejemplo simple: si el producto es un teléfono, recomienda auriculares
            var recomendaciones = new List<Producto>();

            if (producto.Categoria?.Nombre.ToLower().Contains("teléfono") == true)
            {
                var auriculares = await _productoRepository.BuscarAsync("auricular");
                recomendaciones.AddRange(auriculares.Take(max));
            }
            else
            {
                // Si no es teléfono, recomienda productos de la misma categoría
                var similares = await _productoRepository.GetByCategoriaAsync(producto.CategoriaId);
                recomendaciones.AddRange(similares.Where(p => p.ProductoId != producto.ProductoId).Take(max));
            }

            return recomendaciones;
        }
    }
}
