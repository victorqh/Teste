using appTiendaUPN.Models;

namespace appTiendaUPN.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetProductosActivosAsync();
        Task<IEnumerable<Producto>> GetOfertasAsync();
        Task<IEnumerable<Producto>> BuscarProductosAsync(string termino);
        Task<Producto?> GetProductoByIdAsync(int id);
    }
}
