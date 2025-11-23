using appTiendaUPN.Models;
using appTiendaUPN.Repositories;

namespace appTiendaUPN.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepository;
        private readonly IProductoRepository _productoRepository;

        public CarritoService(ICarritoRepository carritoRepository, IProductoRepository productoRepository)
        {
            _carritoRepository = carritoRepository;
            _productoRepository = productoRepository;
        }

        public async Task<Carrito> GetOrCreateCarritoAsync(int userId)
        {
            var carrito = await _carritoRepository.GetByUserIdAsync(userId);
            
            if (carrito == null)
            {
                carrito = new Carrito
                {
                    UserId = userId,
                    FechaCreacion = DateTime.UtcNow
                };
                carrito = await _carritoRepository.CreateAsync(carrito);
            }

            return carrito;
        }

        public async Task<bool> AgregarProductoAsync(int userId, int productoId, int cantidad = 1)
        {
            var producto = await _productoRepository.GetByIdAsync(productoId);
            if (producto == null || !producto.EstaActivo || producto.Stock < cantidad)
                return false;

            var carrito = await GetOrCreateCarritoAsync(userId);
            var itemExistente = await _carritoRepository.GetItemAsync(carrito.CarritoId, productoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
                await _carritoRepository.UpdateItemAsync(itemExistente);
            }
            else
            {
                var nuevoItem = new CarritoItem
                {
                    CarritoId = carrito.CarritoId,
                    ProductoId = productoId,
                    Cantidad = cantidad,
                    Precio = producto.Precio
                };
                await _carritoRepository.AddItemAsync(nuevoItem);
            }

            return true;
        }

        public async Task<bool> ActualizarCantidadAsync(int userId, int productoId, int cantidad)
        {
            if (cantidad <= 0)
                return false;

            var carrito = await _carritoRepository.GetByUserIdAsync(userId);
            if (carrito == null)
                return false;

            var item = await _carritoRepository.GetItemAsync(carrito.CarritoId, productoId);
            if (item == null)
                return false;

            item.Cantidad = cantidad;
            await _carritoRepository.UpdateItemAsync(item);
            return true;
        }

        public async Task<bool> RemoverProductoAsync(int userId, int carritoItemId)
        {
            var carrito = await _carritoRepository.GetByUserIdAsync(userId);
            if (carrito == null)
                return false;

            return await _carritoRepository.RemoveItemAsync(carritoItemId);
        }

        public async Task<bool> VaciarCarritoAsync(int userId)
        {
            var carrito = await _carritoRepository.GetByUserIdAsync(userId);
            if (carrito == null)
                return false;

            return await _carritoRepository.ClearCarritoAsync(carrito.CarritoId);
        }

        public async Task<int> GetCantidadItemsAsync(int userId)
        {
            var carrito = await _carritoRepository.GetByUserIdAsync(userId);
            return carrito?.Items.Sum(i => i.Cantidad) ?? 0;
        }

        public async Task<decimal> GetTotalCarritoAsync(int userId)
        {
            var carrito = await _carritoRepository.GetByUserIdAsync(userId);
            return carrito?.Items.Sum(i => i.Precio * i.Cantidad) ?? 0;
        }
    }
}
