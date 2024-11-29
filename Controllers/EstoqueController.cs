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
    public class EstoqueController : Controller
    {
        private readonly PostgresContext _context;

        public EstoqueController(PostgresContext context)
        {
            _context = context;
        }

        // GET: Estoque
        public async Task<IActionResult> Index()
        {
            var postgresContext = _context.Produtosfornecidos.Include(p => p.IdfornecedorNavigation).Include(p => p.IdprodutoNavigation);
            return View(await postgresContext.ToListAsync());
        }

        // GET: Estoque/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produtosfornecido = await _context.Produtosfornecidos
                .Include(p => p.IdfornecedorNavigation)
                .Include(p => p.IdprodutoNavigation)
                .FirstOrDefaultAsync(m => m.Idproduto == id);
            if (produtosfornecido == null)
            {
                return NotFound();
            }

            return View(produtosfornecido);
        }

        // GET: Estoque/Create
        public IActionResult Create()
        {
            ViewData["Idfornecedor"] = new SelectList(_context.Fornecedors, "Cnpj", "Cnpj");
            ViewData["Idproduto"] = new SelectList(_context.Produtos, "Id", "Id");
            return View();
        }

        // POST: Estoque/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idproduto,Idfornecedor,PrecoCusto,Quantidade,Data")] Produtosfornecido produtosfornecido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produtosfornecido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idfornecedor"] = new SelectList(_context.Fornecedors, "Cnpj", "Cnpj", produtosfornecido.Idfornecedor);
            ViewData["Idproduto"] = new SelectList(_context.Produtos, "Id", "Id", produtosfornecido.Idproduto);
            return View(produtosfornecido);
        }

        // GET: Estoque/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produtosfornecido = await _context.Produtosfornecidos.FindAsync(id);
            if (produtosfornecido == null)
            {
                return NotFound();
            }
            ViewData["Idfornecedor"] = new SelectList(_context.Fornecedors, "Cnpj", "Cnpj", produtosfornecido.Idfornecedor);
            ViewData["Idproduto"] = new SelectList(_context.Produtos, "Id", "Id", produtosfornecido.Idproduto);
            return View(produtosfornecido);
        }

        // POST: Estoque/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idproduto,Idfornecedor,PrecoCusto,Quantidade,Data")] Produtosfornecido produtosfornecido)
        {
            if (id != produtosfornecido.Idproduto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produtosfornecido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutosfornecidoExists(produtosfornecido.Idproduto))
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
            ViewData["Idfornecedor"] = new SelectList(_context.Fornecedors, "Cnpj", "Cnpj", produtosfornecido.Idfornecedor);
            ViewData["Idproduto"] = new SelectList(_context.Produtos, "Id", "Id", produtosfornecido.Idproduto);
            return View(produtosfornecido);
        }

        // GET: Estoque/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produtosfornecido = await _context.Produtosfornecidos
                .Include(p => p.IdfornecedorNavigation)
                .Include(p => p.IdprodutoNavigation)
                .FirstOrDefaultAsync(m => m.Idproduto == id);
            if (produtosfornecido == null)
            {
                return NotFound();
            }

            return View(produtosfornecido);
        }

        // POST: Estoque/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produtosfornecido = await _context.Produtosfornecidos.FindAsync(id);
            if (produtosfornecido != null)
            {
                _context.Produtosfornecidos.Remove(produtosfornecido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutosfornecidoExists(int id)
        {
            return _context.Produtosfornecidos.Any(e => e.Idproduto == id);
        }
    }
}
