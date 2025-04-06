using OP.SysProductos.EN;
using OP.SysProductos.DAL.Implementaciones;

namespace OP.SysProductos.BL
{
    public class ClienteBL
    {
        private readonly ClienteDAL _clienteDAL;

        public ClienteBL(ClienteDAL clienteDAL)
        {
            _clienteDAL = clienteDAL;
        }

        public async Task<int> InsertarAsync(Cliente cliente)
        {
            return await _clienteDAL.InsertarAsync(cliente);
        }

        public async Task<int> ActualizarAsync(Cliente cliente)
        {
            return await _clienteDAL.ActualizarAsync(cliente);
        }

        public async Task<int> EliminarAsync(int id)
        {
            return await _clienteDAL.EliminarAsync(id);
        }

        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            return await _clienteDAL.ObtenerPorIdAsync(id);
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _clienteDAL.ObtenerTodosAsync();
        }
    }
}
