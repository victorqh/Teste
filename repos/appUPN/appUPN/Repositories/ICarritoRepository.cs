using appUPN.Models;

namespace appUPN.Repositories
{
    public interface ICarritoRepository
    {
        Task<Carrito?> ObtenerCarritoPorUserIdAsync(int userId);
        Task<Carrito> CrearCarritoAsync(int userId);
        Task<CarritoItem?> ObtenerItemAsync(int carritoId, int productoId);
        Task AgregarItemAsync(CarritoItem item);
        Task ActualizarItemAsync(CarritoItem item);
        Task EliminarItemAsync(int itemId);
        Task<IEnumerable<CarritoItem>> ObtenerItemsDelCarritoAsync(int carritoId);
        Task<int> ObtenerCantidadItemsAsync(int carritoId);
        Task<decimal> ObtenerTotalCarritoAsync(int carritoId);
    }
}
