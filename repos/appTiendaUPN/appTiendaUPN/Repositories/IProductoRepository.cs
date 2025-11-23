using appTiendaUPN.Models;

namespace appTiendaUPN.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<IEnumerable<Producto>> GetActivosAsync();
        Task<IEnumerable<Producto>> GetOfertasAsync();
        Task<IEnumerable<Producto>> BuscarAsync(string termino);
        Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId);
        Task<Producto?> GetByIdAsync(int id);
        Task<Producto> CreateAsync(Producto producto);
        Task<Producto> UpdateAsync(Producto producto);
        Task<bool> DeleteAsync(int id);
    }
}
