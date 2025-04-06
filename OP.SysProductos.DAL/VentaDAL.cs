using Microsoft.EntityFrameworkCore;
using OP.SysProductos.EN;
using OP.SysProductos.EN.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OP.SysProductos.DAL
{
    public class VentaDAL
    {
        readonly ApplicationDbContext DbContext;

        public VentaDAL(ApplicationDbContext sysProductosDB)
        {
            DbContext = sysProductosDB;
        }

        public async Task<int> CrearAsync(Venta pVenta)
        {
            DbContext.Ventas.Add(pVenta);
            int result = await DbContext.SaveChangesAsync();
            if (result > 0)
            {
                foreach (var detalle in pVenta.DetalleVentas)
                {
                    var producto = await DbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null && producto.CantidadDisponible >= detalle.Cantidad)
                    {
                        producto.CantidadDisponible -= detalle.Cantidad;
                    }
                    else
                    {
                        throw new InvalidOperationException("Stock insuficiente para el producto: " + producto?.Nombre);
                    }
                }
            }
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> AnularAsync(int idVenta)
        {
            var venta = await DbContext.Ventas
                .Include(v => v.DetalleVentas)
                .FirstOrDefaultAsync(v => v.Id == idVenta);

            if (venta != null && venta.Estado != (byte)Venta.EnumEstadoVenta.Anulada)
            {
                // Marca la venta como anulada
                venta.Estado = (byte)Venta.EnumEstadoVenta.Anulada;

                // Restaurar la cantidad de productos al stock
                foreach (var detalle in venta.DetalleVentas)
                {
                    var producto = await DbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible += detalle.Cantidad;
                    }
                }
                return await DbContext.SaveChangesAsync();
            }
            return 0; // Si ya estaba anulada, no hacer nada
        }

        public async Task<Venta> ObtenerPorIdAsync(int idVenta)
        {
            var venta = await DbContext.Ventas
                .Include(v => v.DetalleVentas)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == idVenta);

            return venta ?? new Venta();
        }

        public async Task<List<Venta>> ObtenerTodosAsync()
        {
            var ventas = await DbContext.Ventas
                .Include(v => v.DetalleVentas)
                .Include(v => v.Cliente)
                .ToListAsync();
            return ventas ?? new List<Venta>();
        }

        public async Task<List<Venta>> ObtenerPorEstadoAsync(byte estado)
        {
            var ventasQuery = DbContext.Ventas.AsQueryable();

            if (estado != 0)
            {
                ventasQuery = ventasQuery.Where(v => v.Estado == estado);
            }

            ventasQuery = ventasQuery
                .Include(v => v.DetalleVentas)
                .Include(v => v.Cliente);

            var ventas = await ventasQuery.ToListAsync();

            return ventas ?? new List<Venta>();
        }

        public async Task<List<Venta>> ObtenerReporteVentasAsync(VentaFiltros filtro)
        {
            var ventasQuery = DbContext.Ventas
                .Include(v => v.DetalleVentas)
                .ThenInclude(dv => dv.Producto)
                .Include(v => v.Cliente)
                .AsQueryable();

            if (filtro.FechaInicio.HasValue)
            {
                DateTime fechaInicio = filtro.FechaInicio.Value.Date;
                ventasQuery = ventasQuery.Where(v => v.FechaVenta >= fechaInicio);
            }

            if (filtro.FechaFin.HasValue)
            {
                DateTime fechaFin = filtro.FechaFin.Value.Date.AddDays(1).AddSeconds(-1);
                ventasQuery = ventasQuery.Where(v => v.FechaVenta <= fechaFin);
            }

            return await ventasQuery.ToListAsync();
        }
    }
}