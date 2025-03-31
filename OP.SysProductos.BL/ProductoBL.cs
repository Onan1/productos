using OP.SysProductos.DAL;
using OP.SysProductos.EN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OP.SysProductos.BL
{
    public class ProductoBL
    {
        private readonly ProductoDAL _productoDAL;

        public ProductoBL(ProductoDAL productoDAL)
        {
            _productoDAL = productoDAL;
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _productoDAL.ObtenerTodosAsync();
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            return await _productoDAL.ObtenerPorIdAsync(id);
        }

        public async Task AgregarAsync(Producto producto)
        {
            await _productoDAL.AgregarAsync(producto);
        }

        public async Task EditarAsync(Producto producto)
        {
            await _productoDAL.EditarAsync(producto);
        }

        public async Task EliminarAsync(int id)
        {
            await _productoDAL.EliminarAsync(id);
        }

        public Task AgregarTodosAsync(List<Producto>pProductos)
        {
            return _productoDAL.AgregarTodosAsync(pProductos);
        }
    }
}
