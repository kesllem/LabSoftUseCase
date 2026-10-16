
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;

namespace PetShop.Controllers
{
    [Authorize]
    public class ReservaServicoController : Controller
    {
        private readonly DbPetShopContext _context;

        public ReservaServicoController(DbPetShopContext context)
        {
            _context = context;
        }

        // GET: ReservaServico
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.ReservaServicos
                .Include(r => r.Cliente)
                .Include(r => r.Veterinario)
                .ToListAsync();

            return View(reservas);
        }

        // GET: ReservaServico/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservaServico = await _context.ReservaServicos
                .Include(r => r.Cliente)
                .Include(r => r.Veterinario)
                .FirstOrDefaultAsync(r => r.Codigo == id);

            if (reservaServico == null)
            {
                return NotFound();
            }

            return View(reservaServico);
        }

        // GET: ReservaServico/Create
        public IActionResult Create()
        {
            CarregarListas();
            return View();
        }

        // POST: ReservaServico/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Codigo,DataHora,Valor,StatusServico,ClienteId,VeterinarioId")]
            ReservaServico reservaServico)
        {
            if (ModelState.IsValid)
            {
                _context.ReservaServicos.Add(reservaServico);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            CarregarListas(
                reservaServico.ClienteId,
                reservaServico.VeterinarioId);

            return View(reservaServico);
        }

        // GET: ReservaServico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservaServico =
                await _context.ReservaServicos.FindAsync(id);

            if (reservaServico == null)
            {
                return NotFound();
            }

            CarregarListas(
                reservaServico.ClienteId,
                reservaServico.VeterinarioId);

            return View(reservaServico);
        }

        // POST: ReservaServico/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Codigo,DataHora,Valor,StatusServico,ClienteId,VeterinarioId")]
            ReservaServico reservaServico)
        {
            if (id != reservaServico.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservaServico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaServicoExists(reservaServico.Codigo))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            CarregarListas(
                reservaServico.ClienteId,
                reservaServico.VeterinarioId);

            return View(reservaServico);
        }

        // GET: ReservaServico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservaServico = await _context.ReservaServicos
                .Include(r => r.Cliente)
                .Include(r => r.Veterinario)
                .FirstOrDefaultAsync(r => r.Codigo == id);

            if (reservaServico == null)
            {
                return NotFound();
            }

            return View(reservaServico);
        }

        // POST: ReservaServico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservaServico =
                await _context.ReservaServicos.FindAsync(id);

            if (reservaServico != null)
            {
                _context.ReservaServicos.Remove(reservaServico);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Carrega as listas de clientes e veterinários
        private void CarregarListas(
            int? clienteId = null,
            int? veterinarioId = null)
        {
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes,
                "Codigo",
                "Codigo",
                clienteId);

            ViewData["VeterinarioId"] = new SelectList(
                _context.Veterinarios,
                "Codigo",
                "Codigo",
                veterinarioId);
        }

        // Verifica se a reserva existe
        private bool ReservaServicoExists(int id)
        {
            return _context.ReservaServicos
                .Any(r => r.Codigo == id);
        }
    }
}