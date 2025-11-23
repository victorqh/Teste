using Microsoft.EntityFrameworkCore;
using appTiendaUPN.Data;
using appTiendaUPN.Models;

namespace appTiendaUPN.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly ApplicationDbContext _context;

        public CarritoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Carrito?> GetByUserIdAsync(int userId)
        {
            return await _context.Carritos
                .Include(c => c.Items)
                    .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Carrito> CreateAsync(Carrito carrito)
        {
            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();
            return carrito;
        }

        public async Task<CarritoItem?> GetItemAsync(int carritoId, int productoId)
        {
            return await _context.CarritoItems
                .FirstOrDefaultAsync(i => i.CarritoId == carritoId && i.ProductoId == productoId);
        }

        public async Task<CarritoItem> AddItemAsync(CarritoItem item)
        {
            _context.CarritoItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<CarritoItem> UpdateItemAsync(CarritoItem item)
        {
            _context.CarritoItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> RemoveItemAsync(int carritoItemId)
        {
            var item = await _context.CarritoItems.FindAsync(carritoItemId);
            if (item == null)
                return false;

            _context.CarritoItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCarritoAsync(int carritoId)
        {
            var items = await _context.CarritoItems
                .Where(i => i.CarritoId == carritoId)
                .ToListAsync();

            _context.CarritoItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
