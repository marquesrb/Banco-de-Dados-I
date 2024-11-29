using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjGura.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjGura.Controllers
{
    public class AgendamentosController : Controller
    {
        private readonly PostgresContext _context;

        public AgendamentosController(PostgresContext context)
        {
            _context = context;
        }

        // GET: Agendamentos
        public async Task<IActionResult> Index([FromQuery]string? Idbanhista, [FromQuery]DateOnly? dtaIni, [FromQuery]DateOnly? dtaFim) 
        {
           
            var consulta = _context.Servicos
                                    .Include(s => s.IdbanhistaNavigation)
                                    .Include(s => s.IdcaixaNavigation)
                                    .Include(s => s.IdpetNavigation)
                                    .Include(s => s.IdvendaNavigation)
                                    .AsQueryable();
            if (dtaIni != null && dtaFim != null)
            {
                consulta = consulta.Where(s => s.Data >= dtaIni && s.Data <= dtaFim);
            }

            if (Idbanhista != null)
            {
                consulta = consulta.Where(s => s.Idbanhista.Equals(Idbanhista));
            }
            //if (IdCliente != null)
            //{
            //     consulta = _context.Servicos
            //        .Join(_context.Pets, a => a.Idpet, p => p.Idpet, (a, p) => new { a, p })
            //        .Where(x => x.p.Idcliente == IdCliente)
            //        .Select(x => x.a);

            //}
            return View(await consulta.ToListAsync());
        }

        // GET: Agendamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos
                .Include(s => s.IdbanhistaNavigation)
                .Include(s => s.IdcaixaNavigation)
                .Include(s => s.IdpetNavigation)
                .Include(s => s.IdvendaNavigation)
                .FirstOrDefaultAsync(m => m.Idservico == id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        // GET: Agendamentos/Create
        public IActionResult Create()
        {
            ViewData["Idbanhista"] = new SelectList(_context.Funcionarios, "Cpf", "Cpf");
            ViewData["Idcaixa"] = new SelectList(_context.Funcionarios, "Cpf", "Cpf");
            ViewData["Idpet"] = new SelectList(_context.Pets, "Idpet", "Idpet");
            ViewData["Idvenda"] = new SelectList(_context.Venda, "Id", "Id");
            return View();
        }

        // POST: Agendamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idservico,Tipo,Preco,Data,Horario,Status,Idcaixa,Idpet,Idbanhista,Idvenda")] Servico servico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idbanhista"] = new SelectList(_context.Funcionarios, "Cpf", "Cpf", servico.Idbanhista);
            ViewData["Idcaixa"] = new SelectList(_context.Funcionarios, "Cpf", "Cpf", servico.Idcaixa);
            ViewData["Idpet"] = new SelectList(_context.Pets, "Idpet", "Idpet", servico.Idpet);
            ViewData["Idvenda"] = new SelectList(_context.Venda, "Id", "Id", servico.Idvenda);
            return View(servico);
        }

        // GET: Agendamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null)
            {
                return NotFound();
            }
            ViewData["Idbanhista"] = new SelectList(_context.Funcionarios, "Cpf", "Cpf", servico.Idbanhista);
            ViewData["Idcaixa"] = new SelectList(_context.Funcionarios, "Cpf", "Cpf", servico.Idcaixa);
            ViewData["Idpet"] = new SelectList(_context.Pets, "Idpet", "Idpet", servico.Idpet);
            ViewData["Idvenda"] = new SelectList(_context.Venda, "Id", "Id", servico.Idvenda);
            return View(servico);
        }

        // POST: Agendamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idservico,Tipo,Preco,Data,Horario,Status,Idcaixa,Idpet,Idbanhista,Idvenda")] Servico servico)
        {
            if (id != servico.Idservico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicoExists(servico.Idservico))
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
            ViewData["Idbanhista"] = new SelectList(_context.Funcionarios, "Cpf", "Cpf", servico.Idbanhista);
            ViewData["Idcaixa"] = new SelectList(_context.Funcionarios, "Cpf", "Cpf", servico.Idcaixa);
            ViewData["Idpet"] = new SelectList(_context.Pets, "Idpet", "Idpet", servico.Idpet);
            ViewData["Idvenda"] = new SelectList(_context.Venda, "Id", "Id", servico.Idvenda);
            return View(servico);
        }

        // GET: Agendamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos
                .Include(s => s.IdbanhistaNavigation)
                .Include(s => s.IdcaixaNavigation)
                .Include(s => s.IdpetNavigation)
                .Include(s => s.IdvendaNavigation)
                .FirstOrDefaultAsync(m => m.Idservico == id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        // POST: Agendamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico != null)
            {
                _context.Servicos.Remove(servico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicoExists(int id)
        {
            return _context.Servicos.Any(e => e.Idservico == id);
        }
    }
}
