using appTiendaUPN.Models;

namespace appTiendaUPN.Repositories
{
    public interface ICarritoRepository
    {
        Task<Carrito?> GetByUserIdAsync(int userId);
        Task<Carrito> CreateAsync(Carrito carrito);
        Task<CarritoItem?> GetItemAsync(int carritoId, int productoId);
        Task<CarritoItem> AddItemAsync(CarritoItem item);
        Task<CarritoItem> UpdateItemAsync(CarritoItem item);
        Task<bool> RemoveItemAsync(int carritoItemId);
        Task<bool> ClearCarritoAsync(int carritoId);
    }
}
