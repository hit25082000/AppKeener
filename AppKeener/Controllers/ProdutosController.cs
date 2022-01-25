using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppKeener.Data;
using AppKeener.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace AppKeener.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Produtos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            foreach (var pro in _context.Produtos)
            {
                var produtos = _context.Estoques.FirstOrDefault(a => a.ProdutoId == pro.Id);
                if (produtos != null)
                {
                    foreach (var mov in _context.Estoques)
                    {
                        var prodId = mov.ProdutoId.Equals(_context.Produtos.Find(mov.ProdutoId).Id);
                        var Quant = _context.Estoques.Where(a => a.ProdutoId == mov.ProdutoId);
                        var Total = Quant.Sum(item => item.Recebido - item.Enviado);

                        if (prodId)
                        {
                            _context.Produtos.Find(mov.ProdutoId).Quantidade = Total;
                        }
                    }
                }
                else
                {
                    pro.Quantidade = 0;
                }
            }
            await _context.SaveChangesAsync();

            return View(await _context.Produtos.ToListAsync());
        }

        // GET: Produtos/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
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

        // GET: Produtos/Create
        public IActionResult Create()
        {            
            return View();
        }

        // POST: Produtos/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            if (ModelState.IsValid)
            {
                produto.Id = Guid.NewGuid();
                produto.Quantidade = 0;
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Produto produto)
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
        public async Task<IActionResult> Delete(Guid? id)
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
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await _context.Produtos.FindAsync(id);            

            foreach (var mov in _context.Estoques)
            {    
                if (mov.Id.Equals(_context.Produtos.Find(produto.Id))){

                    _context.Estoques.Remove(_context.Estoques.Find(mov.Id));
                    
                }               
                
            }
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(Guid id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
