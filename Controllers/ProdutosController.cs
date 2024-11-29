using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjGura.Models;
using prjGura.ViewModel;

namespace prjGura.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly PostgresContext _context;

        public ProdutosController(PostgresContext context)
        {
            _context = context;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Produtos.ToListAsync());
        }
        public async Task<IActionResult> Vendas([FromQuery]string? nomeProd, [FromQuery] DateOnly? dtaIni, [FromQuery] DateOnly? dtaFim)
        {
            var resultado = await _context.Produtos
                            .Join(
                                _context.Vendadeprodutos, // Tabela VendaProduto
                                p => p.Id,               // Chave primária de Produto
                                vp => vp.Idproduto,      // Chave estrangeira em VendaProduto
                                (p, vp) => new { p, vp } // Resultado da junção Produto com VendaProduto
                            )
                            .Join(
                                _context.Venda,          // Tabela Venda
                                temp => temp.vp.Idvenda, // Chave estrangeira em VendaProduto
                                v => v.Id,               // Chave primária de Venda
                                (temp, v) => new { temp.p, temp.vp, v } // Resultado da junção com Venda
                            )
                            .Join(
                                _context.Clientes,       // Tabela Clientes
                                temp => temp.v.Idcliente,      // Chave estrangeira em Venda
                                c => c.Cpf,              // Chave primária de Clientes
                                (temp, c) => new VendasViewModel        // Resultado final da junção
                                {
                                    ProdutoNome = temp.p.Nome,
                                    ClienteNome = c.Nome,
                                    DataVenda = temp.v.Data,
                                    QuantidadeVendida = temp.vp.Quantidadevendida
                                }
                            )
                            .Where(result => string.IsNullOrEmpty(nomeProd) || result.ProdutoNome.Contains(nomeProd))
                            .Where(result =>
                                (!dtaIni.HasValue || result.DataVenda >= dtaIni.Value) &&
                                (!dtaFim.HasValue || result.DataVenda <= dtaFim.Value)
                            )
                            .ToListAsync(); // Executa a consulta e retorna a lista
            //(!dataInicio.HasValue || v.Data >= dataInicio.Value) && // Filtro por data inicial
            //(!dataFim.HasValue || v.Data <= dataFim.Value)         // Filtro por data final
            return View(resultado);
        }


        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }
            var listprdforn = _context.Produtosfornecidos.Where(x => x.Idproduto == produto.Id).ToList();

            produto.Produtosfornecidos = listprdforn;


            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,PrecoVenda")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,PrecoVenda")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
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
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
