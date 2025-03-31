using OP.SysProductos.EN;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OP.SysProductos.DAL
{
    public class ProductoDAL
    {
        private readonly ApplicationDbContext _context;

        public ProductoDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task AgregarAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();
        }

        public async Task EditarAsync(Producto producto)
        {
            var productoExistente = await _context.Productos.FindAsync(producto.Id);
            if (productoExistente != null)
            {
                productoExistente.Nombre = producto.Nombre;
                productoExistente.Precio = producto.Precio;
                productoExistente.CantidadDisponible = producto.CantidadDisponible;

                _context.Productos.Update(productoExistente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AgregarTodosAsync(List<Producto> pProductos)
        {
            await _context.Productos.AddRangeAsync(pProductos);
            await _context.SaveChangesAsync();
        }
    }
}
