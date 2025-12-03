using appUPN.Data;
using appUPN.Models;
using Microsoft.EntityFrameworkCore;

namespace appUPN.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.EstaActivo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.EstaActivo && p.CategoriaId == categoriaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> ObtenerOfertasAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.EstaActivo && p.EsOferta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> ObtenerOfertasDestacadasAsync(int cantidad)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.EstaActivo && p.EsOferta)
                .Take(cantidad)
                .ToListAsync();
        }

        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.ProductoId == id);
        }

        public async Task<IEnumerable<Categoria>> ObtenerCategoriasAsync()
        {
            return await _context.Categorias.ToListAsync();
        }
    }
}
