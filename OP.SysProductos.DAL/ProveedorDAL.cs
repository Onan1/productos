using Microsoft.EntityFrameworkCore;
using OP.SysProductos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.SysProductos.DAL
{
    public class ProveedorDAL
    {
        readonly ApplicationDbContext DbContext;

        public ProveedorDAL(ApplicationDbContext sysProductosDB)
        {
            DbContext = sysProductosDB;
        }

        public async Task<int> CrearAsync(Proveedor pPrveedor)
        {
            Proveedor proveedor = new Proveedor()
            {
                Nombre = pPrveedor.Nombre,
                NRC = pPrveedor.NRC,
                Direccion = pPrveedor.Direccion,
                Telefono = pPrveedor.Telefono,
                Email = pPrveedor.Email
            };
            DbContext.Proveedores.Add(proveedor);
            return await DbContext.SaveChangesAsync();
        }
        public async Task<int> EliminarAsync(Proveedor pProveedor)
        {
            var proveedor = await DbContext.Proveedores.FirstOrDefaultAsync(s => s.Id == pProveedor.Id);
            if (proveedor != null && proveedor.Id != 0)
            {
                DbContext.Proveedores.Remove(proveedor);
                return await DbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<int> ModificarAsync(Proveedor pProveedor)
        {
            var proveedor = await DbContext.Proveedores.FirstOrDefaultAsync(s => s.Id == pProveedor.Id);
            if (proveedor != null && proveedor.Id != 0)
            {
                proveedor.Nombre = pProveedor.Nombre;
                proveedor.NRC = pProveedor.NRC;
                proveedor.Direccion = pProveedor.Direccion;
                proveedor.Telefono = pProveedor.Telefono;
                proveedor.Email = pProveedor.Email;

                DbContext.Update(proveedor);
                return await DbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<Proveedor> ObtenerPorIdAsync(Proveedor pProveedor)
        {
            var proveedor = await DbContext.Proveedores.FirstOrDefaultAsync(s => s.Id == pProveedor.Id);
            if (proveedor != null && proveedor.Id != 0)
            {
                return new Proveedor
                {
                    Id = proveedor.Id,
                    Nombre = proveedor.Nombre,
                    NRC = proveedor.NRC,
                    Direccion = proveedor.Direccion,
                    Telefono = proveedor.Telefono,
                    Email = pProveedor.Email,
                };

            }
            else
                return new Proveedor();
        }
        public async Task<List<Proveedor>> ObtenerTodosAsync()
        {
            var  Proveedores = await DbContext.Proveedores.ToListAsync();
            if (Proveedores != null && Proveedores.Count > 0)
            {
                var list = new List<Proveedor>();
                Proveedores.ForEach(p => list.Add(new Proveedor
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    NRC = p.NRC,
                    Direccion = p.Direccion,
                    Telefono = p.Telefono,
                    Email = p.Email
                }));
                return list;
            }
            else
                return new List<Proveedor>();
        }

        public async Task AgregarTodosAsync(List<Proveedor> pProveedor)
        {
            await DbContext.Proveedores.AddRangeAsync(pProveedor);
            await DbContext.SaveChangesAsync();
        }

    }
}
