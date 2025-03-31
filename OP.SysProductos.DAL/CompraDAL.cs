using Microsoft.EntityFrameworkCore;
using OP.SysProductos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.SysProductos.DAL
{
    public class CompraDAL
    {
        readonly ApplicationDbContext DbContext;

        public CompraDAL(ApplicationDbContext SysProdctosDB)
        {
            DbContext = SysProdctosDB;
        }

        public async Task<int> CrearAsync(Compra pCompra)
        {
            DbContext.Compras.Add(pCompra);
            int result = await DbContext.SaveChangesAsync();
            if (result > 0)
            {
                foreach (var detalle in pCompra.DetalleCompras)
                {
                    var producto = await DbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible += detalle.Cantidad;
                    }
                }
            }
            return await DbContext.SaveChangesAsync();
        }
        public async Task<int> AnularAsync(int idCompra)
        {
            var compra = await DbContext.Compras
                .Include(c => c.DetalleCompras)
                .FirstOrDefaultAsync(c => c.Id == idCompra);

            if (compra != null && compra.Estado != (byte)Compra.EnumEstadoCompra.Anulada)
            {
                //Marca la compra como anulada
                compra.Estado = (byte)Compra.EnumEstadoCompra.Anulada;

                //Restar la cantidad como anulada
                foreach (var detalle in compra.DetalleCompras)
                {
                    var producto = await DbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible -= detalle.Cantidad;
                    }
                }
                return await DbContext.SaveChangesAsync();
            }
            return 0; //Si ya estaba anulada, no hacer nada
        }
        public async Task<Compra> ObtenerPorIdAsync(int idCompra)
        {
            var compra = await DbContext.Compras
                .Include(c => c.DetalleCompras).Include(c => c.Proveedor)
                .FirstOrDefaultAsync(c => c.Id == idCompra);

            return compra ?? new Compra();
        }
        public async Task<List<Compra>> ObtenerTodosAsync()
        {
            var compras = await DbContext.Compras
                .Include(c => c.DetalleCompras)
                .Include(c => c.Proveedor).ToListAsync();
            return compras ?? new List<Compra>();
        }

        public async Task<List<Compra>> ObtenerPorEstadoAsync(byte estado)
        {
            var comprasQuery = DbContext.Compras.AsQueryable();

            if (estado != 0)
            {
                comprasQuery = comprasQuery.Where(c => c.Estado == estado);
            }

            comprasQuery = comprasQuery
                .Include(c => c.DetalleCompras)
                .Include(c => c.Proveedor);

            var compras = await comprasQuery.ToListAsync();

            return compras ?? new List<Compra>();
        }
    }
}
