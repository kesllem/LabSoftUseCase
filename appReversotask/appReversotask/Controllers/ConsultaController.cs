using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using appReversotask.Models;

namespace appReversotask.Controllers
{
    [Authorize] // Impede o acesso de usuários não autenticados
    public class ConsultaController : Controller
    {
        private readonly DbclinicaContext _context;

        public ConsultaController(DbclinicaContext context)
        {
            _context = context;
        }

        // GET: Consulta
        public async Task<IActionResult> Index()
        {
            var pacienteId = HttpContext.Session.GetInt32("PacienteId");

            if (pacienteId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var consultas = await _context.Consulta
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .Where(c => c.PacienteId == pacienteId)
                .ToListAsync();

            return View(consultas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var consultum = await _context.Consulta
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (consultum == null) return NotFound();

            return View(consultum);
        }

        public IActionResult Create()
        {
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "Codigo", "Codigo");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Codigo", "Codigo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,DataHora,StatusConsulta,PacienteId,MedicoId")] Consultum consultum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consultum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "Codigo", "Codigo", consultum.MedicoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Codigo", "Codigo", consultum.PacienteId);
            return View(consultum);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var consultum = await _context.Consulta.FindAsync(id);
            if (consultum == null) return NotFound();

            ViewData["MedicoId"] = new SelectList(_context.Medicos, "Codigo", "Codigo", consultum.MedicoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Codigo", "Codigo", consultum.PacienteId);
            return View(consultum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,DataHora,StatusConsulta,PacienteId,MedicoId")] Consultum consultum)
        {
            if (id != consultum.Codigo) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultumExists(consultum.Codigo)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "Codigo", "Codigo", consultum.MedicoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Codigo", "Codigo", consultum.PacienteId);
            return View(consultum);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var consultum = await _context.Consulta
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (consultum == null) return NotFound();

            return View(consultum);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultum = await _context.Consulta.FindAsync(id);
            if (consultum != null)
            {
                _context.Consulta.Remove(consultum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultumExists(int id)
        {
            return _context.Consulta.Any(e => e.Codigo == id);
        }
    }
}