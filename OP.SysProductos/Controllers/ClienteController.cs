using Microsoft.AspNetCore.Mvc;
using OP.SysProductos.BL;
using OP.SysProductos.EN;
using System.Threading.Tasks;

namespace OP.SysProductos.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteBL _clienteBL;

        public ClienteController(ClienteBL clienteBL)
        {
            _clienteBL = clienteBL;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            return View(clientes);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteBL.InsertarAsync(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteBL.ActualizarAsync(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteBL.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }
    }
}
