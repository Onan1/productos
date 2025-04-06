using Microsoft.EntityFrameworkCore;
using OP.SysProductos.EN;

namespace OP.SysProductos.DAL.Implementaciones
{
    public class ClienteDAL
    {
        private readonly ApplicationDbContext _context;

        public ClienteDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> InsertarAsync(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ActualizarAsync(Cliente cliente)
        {
            _context.Cliente.Update(cliente);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> EliminarAsync(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            return await _context.Cliente.FindAsync(id);
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _context.Cliente.ToListAsync();
        }
    }
}
