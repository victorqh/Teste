using appUPN.Data;
using appUPN.Models;
using Microsoft.EntityFrameworkCore;

namespace appUPN.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly AppDbContext _context;

        public CarritoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Carrito?> ObtenerCarritoPorUserIdAsync(int userId)
        {
            return await _context.Carritos
                .Include(c => c.Items)
                    .ThenInclude(i => i.Producto)
                        .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Carrito> CrearCarritoAsync(int userId)
        {
            var carrito = new Carrito
            {
                UserId = userId,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();
            return carrito;
        }

        public async Task<CarritoItem?> ObtenerItemAsync(int carritoId, int productoId)
        {
            return await _context.CarritoItems
                .FirstOrDefaultAsync(i => i.CarritoId == carritoId && i.ProductoId == productoId);
        }

        public async Task AgregarItemAsync(CarritoItem item)
        {
            _context.CarritoItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarItemAsync(CarritoItem item)
        {
            _context.CarritoItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarItemAsync(int itemId)
        {
            var item = await _context.CarritoItems.FindAsync(itemId);
            if (item != null)
            {
                _context.CarritoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CarritoItem>> ObtenerItemsDelCarritoAsync(int carritoId)
        {
            return await _context.CarritoItems
                .Include(i => i.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(i => i.CarritoId == carritoId)
                .ToListAsync();
        }

        public async Task<int> ObtenerCantidadItemsAsync(int carritoId)
        {
            return await _context.CarritoItems
                .Where(i => i.CarritoId == carritoId)
                .SumAsync(i => i.Cantidad);
        }

        public async Task<decimal> ObtenerTotalCarritoAsync(int carritoId)
        {
            return await _context.CarritoItems
                .Where(i => i.CarritoId == carritoId)
                .SumAsync(i => i.Precio * i.Cantidad);
        }
    }
}
