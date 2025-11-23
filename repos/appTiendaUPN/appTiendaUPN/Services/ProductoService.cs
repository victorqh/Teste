using appTiendaUPN.Models;
using appTiendaUPN.Repositories;

namespace appTiendaUPN.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<IEnumerable<Producto>> GetProductosActivosAsync()
        {
            return await _productoRepository.GetActivosAsync();
        }

        public async Task<IEnumerable<Producto>> GetOfertasAsync()
        {
            return await _productoRepository.GetOfertasAsync();
        }

        public async Task<IEnumerable<Producto>> BuscarProductosAsync(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return await GetProductosActivosAsync();

            return await _productoRepository.BuscarAsync(termino);
        }

        public async Task<Producto?> GetProductoByIdAsync(int id)
        {
            return await _productoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(int categoriaId)
        {
            return await _productoRepository.GetByCategoriaAsync(categoriaId);
        }
    }
}
