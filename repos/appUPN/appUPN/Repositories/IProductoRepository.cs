using appUPN.Models;

namespace appUPN.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Producto>> ObtenerOfertasAsync();
        Task<IEnumerable<Producto>> ObtenerOfertasDestacadasAsync(int cantidad);
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Categoria>> ObtenerCategoriasAsync();
    }
}
