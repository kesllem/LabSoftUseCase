using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppTask.Models;
using AppTask.Models.Services;

namespace AppTask.Controllers
{
    public class TarefaController : Controller
    {
        private readonly DbTasksContext _context;
        private RegraTarefa _regraTarefa;

        public TarefaController(DbTasksContext context)
        {
            _context = context;
            _regraTarefa = new RegraTarefa();
        }

        // GET: Tarefa
        public async Task<IActionResult> Index()
        {
            var tarefas = await _context.Tarefa.ToListAsync();
            return View(tarefas);
        }

        public async Task<IActionResult> Sobre()
        {

            return View();
        }

        // GET: Tarefa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefa
                .Include(t => t.Funcionario)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        // GET: Tarefa/Create
        public IActionResult Create()
        {
            ViewData["ListaFuncionario"] = new SelectList(_context.Funcionario, "Codigo", "Nome");

            return View();
        }

        // POST: Tarefa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("Codigo,Descricao,DataPlanejada,DataIniciada,DataFinalizada,DataCancelada,StatusTarefa,Prazo,FuncionarioId")] Tarefa tarefa)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var erro in item.Value.Errors)
                    {
                        Console.WriteLine($"Campo: {item.Key} - Erro: {erro.ErrorMessage}");
                    }
                }

                ViewData["ListaFuncionario"] = new SelectList(
                    _context.Funcionario,
                    "Codigo",
                    "Nome",
                    tarefa.FuncionarioId
                );

                return View(tarefa);
            }

            _context.Tarefa.Add(tarefa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // GET: Tarefa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefa.FindAsync(id);
            if (tarefa == null)
            {
                return NotFound();
            }
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionario, "Codigo", "Nome", tarefa.FuncionarioId);
            return View(tarefa);
        }

        // POST: Tarefa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Descricao,DataPlanejada,DataIniciada,DataFinalizada,DataCancelada,StatusTarefa,Prazo,FuncionarioId")] Tarefa tarefa)
        {
            if (id != tarefa.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid & _regraTarefa.validarDataFinal(tarefa.DataIniciada, tarefa.DataFinalizada))
            {
                try
                {
                    _context.Update(tarefa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefaExists(tarefa.Codigo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionario, "Codigo", "Nome", tarefa.FuncionarioId);
            return View(tarefa);
        }

        // GET: Tarefa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefa
                .Include(t => t.Funcionario)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        // POST: Tarefa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarefa = await _context.Tarefa.FindAsync(id);
            if (tarefa != null)
            {
                _context.Tarefa.Remove(tarefa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TarefaExists(int id)
        {
            return _context.Tarefa.Any(e => e.Codigo == id);
        }
    }
}
