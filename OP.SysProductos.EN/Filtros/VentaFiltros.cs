using System;

namespace OP.SysProductos.EN.Filtros
{
    public class VentaFiltros
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public byte TipoReporte { get; set; }
        public int? IdCliente { get; set; }

        public enum EnumTipoReporte
        {
            PDF = 1,
            Excel = 2
        }
    }
}
