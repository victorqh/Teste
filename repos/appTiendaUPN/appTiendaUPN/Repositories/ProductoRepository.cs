using Microsoft.EntityFrameworkCore;
using appTiendaUPN.Data;
using appTiendaUPN.Models;

namespace appTiendaUPN.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetActivosAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.EstaActivo && p.Stock > 0)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetOfertasAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.EstaActivo && p.EsOferta && p.Stock > 0)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> BuscarAsync(string termino)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.EstaActivo && 
                       (p.Nombre.ToLower().Contains(termino.ToLower()) || 
                        p.Descripcion!.ToLower().Contains(termino.ToLower())))
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.EstaActivo && p.CategoriaId == categoriaId && p.Stock > 0)
                .ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.ProductoId == id);
        }

        public async Task<Producto> CreateAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto> UpdateAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
