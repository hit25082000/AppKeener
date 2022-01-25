using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppKeener.Data;
using AppKeener.Models;
using Microsoft.AspNetCore.Authorization;

namespace AppKeener.Controllers
{
    [Authorize]
    public class EstoqueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstoqueController(ApplicationDbContext context)
        {
            _context = context;
        }        

        // GET: Estoque
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //
            var QuantEnv = _context.Estoques.Sum(x => x.Enviado);         
            var QuantRec = _context.Estoques.Sum(x => x.Recebido);
            //
            var applicationDbContext = _context.Estoques.Include(e => e.Produto);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Estoque/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estoque = await _context.Estoques
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estoque == null)
            {
                return NotFound();
            }

            return View(estoque);
        }

        // GET: Estoque/Create
        public IActionResult Create()
        {
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome");
            return View();
        }

        // POST: Estoque/Create   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estoque estoque)
        {
            var Quant = _context.Produtos.Find(estoque.ProdutoId).Quantidade;
            var Total = Quant + estoque.Recebido;
            if (Total >= estoque.Enviado)
            {
                if (ModelState.IsValid)
                {
                    estoque.Id = Guid.NewGuid();
                    _context.Add(estoque);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                Console.Beep();
            }
            
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", estoque.ProdutoId);
            return View(estoque);
        }

        // GET: Estoque/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estoque = await _context.Estoques.FindAsync(id);
            if (estoque == null)
            {
                return NotFound();
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", estoque.ProdutoId);
            return View(estoque);
        }

        // POST: Estoque/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Estoque estoque)
        {
            if (id != estoque.Id)
            {
                return NotFound();
            }

            var Quant = _context.Produtos.Find(estoque.ProdutoId).Quantidade;
            var Total = Quant + estoque.Recebido;
            if (Total >= estoque.Enviado)
            {
                if (ModelState.IsValid)
                {
                    try
                        {
                        _context.Update(estoque);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!EstoqueExists(estoque.Id))
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
            }
            else
            {
                Console.Beep();
            }

            
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", estoque.ProdutoId);
            return View(estoque);
        }

        // GET: Estoque/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estoque = await _context.Estoques
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estoque == null)
            {
                return NotFound();
            }

            return View(estoque);
        }

        // POST: Estoque/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var estoque = await _context.Estoques.FindAsync(id);
            _context.Estoques.Remove(estoque);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstoqueExists(Guid id)
        {
            return _context.Estoques.Any(e => e.Id == id);
        }
    }
}
