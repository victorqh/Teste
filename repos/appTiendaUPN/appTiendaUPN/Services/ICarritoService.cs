using appTiendaUPN.Models;

namespace appTiendaUPN.Services
{
    public interface ICarritoService
    {
        Task<Carrito> GetOrCreateCarritoAsync(int userId);
        Task<bool> AgregarProductoAsync(int userId, int productoId, int cantidad = 1);
        Task<bool> ActualizarCantidadAsync(int userId, int productoId, int cantidad);
        Task<bool> RemoverProductoAsync(int userId, int carritoItemId);
        Task<bool> VaciarCarritoAsync(int userId);
        Task<int> GetCantidadItemsAsync(int userId);
        Task<decimal> GetTotalCarritoAsync(int userId);
    }
}
