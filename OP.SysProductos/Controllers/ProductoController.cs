using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OP.SysProductos.BL;
using OP.SysProductos.EN;
using Rotativa.AspNetCore;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OP.SysProductos.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoBL _productoBL;

        public ProductoController(ProductoBL productoBL)
        {
            _productoBL = productoBL;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoBL.ObtenerTodosAsync();
            return View(productos);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.FechaCreacion = DateTime.Now;
                await _productoBL.AgregarAsync(producto);
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var producto = await _productoBL.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Obtener la fecha original desde la base de datos
                var productoExistente = await _productoBL.ObtenerPorIdAsync(id);
                if (productoExistente == null)
                {
                    return NotFound();
                }

                // Mantener la fecha de creación original
                producto.FechaCreacion = productoExistente.FechaCreacion;

                await _productoBL.EditarAsync(producto);
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }



        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _productoBL.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            var producto = await _productoBL.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            await _productoBL.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> ReporteProductos()
        {
            var  productos = await _productoBL.ObtenerTodosAsync();
            return new ViewAsPdf("rpProductos", productos);
        }
        public async Task<ActionResult> ProductosJson()
        {
            var productos = await _productoBL.ObtenerTodosAsync();
            var productosData = productos
                .Select(p => new
                {
                    nombre = p.Nombre,
                    stock = p.CantidadDisponible
                })
                .ToList();
            return Json(productosData);
        }
        public async Task<ActionResult> ProductosJsonPrecio()
        {
            var productos = await _productoBL.ObtenerTodosAsync();
            var productosData = productos
                .Select(p => new
                {
                    fechaCreacion = p.FechaCreacion.ToString("yyyy-MM-dd"),
                    precio = p.Precio
                })
                .ToList();

            var groupedData = productosData
                .GroupBy(p => p.fechaCreacion)
                .Select(g => new
                {
                    fecha = g.Key,
                    precioPromedio = g.Average(p => p.precio)
                })
                .OrderBy(g => g.fecha)
                .ToList();

            return Json(groupedData);
        }

        public async Task<ActionResult> ReporteProductosExcel()
        {
            var productos = await _productoBL.ObtenerTodosAsync();
            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Productos");

                hojaExcel.Cells["A1"].Value = "Nombre";
                hojaExcel.Cells["B1"].Value = "Precio";
                hojaExcel.Cells["C1"].Value = "Cantidad";
                hojaExcel.Cells["D1"].Value = "Fecha";

                int row = 2;

                foreach (var producto in productos)
                {
                    hojaExcel.Cells[row, 1].Value = producto.Nombre;
                    hojaExcel.Cells[row, 2].Value = producto.Precio;
                    hojaExcel.Cells[row, 3].Value = producto.CantidadDisponible;
                    hojaExcel.Cells[row, 4].Value = producto.FechaCreacion.ToString("yyyy-MM-dd");
                    row++;
                }

                hojaExcel.Cells["A:D"].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "aplication/vnd.openxalformarts.officedocument.spreadsheet.sheet", "ReportePorudctosExcel.xlsx");
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> SubirExcelProductos(IFormFile archivoExel)
        {
            if (archivoExel == null || archivoExel.Length == 0)
            {
                return RedirectToAction("Index");
            }

            var productos = new List<Producto>();

            using (var stream = new MemoryStream())
            {
                await archivoExel.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var hojaExcel = package.Workbook.Worksheets[0];

                    int rowCount = hojaExcel.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var nombre = hojaExcel.Cells[row, 1].Text;
                        var precio = hojaExcel.Cells[row, 2].GetValue<decimal>();
                        var cantidad = hojaExcel.Cells[row, 3].GetValue<int>();
                        var fecha = hojaExcel.Cells[row, 4].GetValue<DateTime>();

                        if (string.IsNullOrEmpty(nombre) || precio <= 0 || cantidad < 0)
                            continue;
                        productos.Add(new Producto
                        {
                            Nombre = nombre,
                            Precio = precio,
                            CantidadDisponible = cantidad,
                            FechaCreacion = fecha
                        });
                    }
                }
                if (productos.Count > 0)
                {
                    await _productoBL.AgregarTodosAsync(productos);
                }
                return RedirectToAction("Index");
            }

        }

    }
}
