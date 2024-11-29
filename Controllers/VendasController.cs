using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjGura.Models;

namespace prjGura.Controllers
{
    public class VendasController : Controller
    {
        private readonly PostgresContext _context;

        public VendasController(PostgresContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index([FromQuery] string? id)
        {
            if (id == null)
            {
                var servico2 = _context.Venda
                            .Select(v => new Vendum
                            {
                                Id = v.Id,
                                Valortotal = v.Valortotal,
                                Data = v.Data,
                                Idcliente = v.Idcliente,
                                Servicos = v.Servicos.Select(s => new Servico
                                {
                                    Idservico = s.Idservico,
                                    Tipo = s.Tipo,
                                    Preco = s.Preco,
                                    Data = s.Data,
                                    Horario = s.Horario,
                                    Status = s.Status,
                                    Idcaixa = s.Idcaixa,
                                    Idpet = s.Idpet,
                                    Idbanhista = s.Idbanhista,
                                    Idvenda = s.Idvenda
                                }).ToList()
                            }).ToList();
                return View(servico2);

            }

            var servico = _context.Venda
                        .Where(v => v.Idcliente == id)
                        .Select(v => new Vendum
                        {
                            Id = v.Id,
                            Valortotal = v.Valortotal,
                            Data = v.Data,
                            Idcliente = v.Idcliente,
                            Servicos = v.Servicos.Select(s => new Servico
                            {
                                Idservico = s.Idservico,
                                Tipo = s.Tipo,
                                Preco = s.Preco,
                                Data = s.Data,
                                Horario = s.Horario,
                                Status = s.Status,
                                Idcaixa = s.Idcaixa,
                                Idpet = s.Idpet,
                                Idbanhista = s.Idbanhista,
                                Idvenda = s.Idvenda
                            }).ToList()
                        }).ToList();

            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
            //var postgresContext = _context.Venda.Include(v => v.IdclienteNavigation);
            //return View(await postgresContext.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendum = await _context.Venda
                .Include(v => v.IdclienteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendum == null)
            {
                return NotFound();
            }

            return View(vendum);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            ViewData["Idcliente"] = new SelectList(_context.Clientes, "Cpf", "Cpf");
            return View();
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Valortotal,Data,Idcliente")] Vendum vendum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idcliente"] = new SelectList(_context.Clientes, "Cpf", "Cpf", vendum.Idcliente);
            return View(vendum);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendum = await _context.Venda.FindAsync(id);
            if (vendum == null)
            {
                return NotFound();
            }
            ViewData["Idcliente"] = new SelectList(_context.Clientes, "Cpf", "Cpf", vendum.Idcliente);
            return View(vendum);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Valortotal,Data,Idcliente")] Vendum vendum)
        {
            if (id != vendum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendumExists(vendum.Id))
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
            ViewData["Idcliente"] = new SelectList(_context.Clientes, "Cpf", "Cpf", vendum.Idcliente);
            return View(vendum);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendum = await _context.Venda
                .Include(v => v.IdclienteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendum == null)
            {
                return NotFound();
            }

            return View(vendum);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendum = await _context.Venda.FindAsync(id);
            if (vendum != null)
            {
                _context.Venda.Remove(vendum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendumExists(int id)
        {
            return _context.Venda.Any(e => e.Id == id);
        }
    }
}
