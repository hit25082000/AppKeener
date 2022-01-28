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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AppKeener.Controllers
{
    [Authorize]
    public class EstoqueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EstoqueController(
            ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Estoque
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
                        var produtoId = mov.ProdutoId.Equals(_context.Produtos.Find(mov.ProdutoId).Id);
                        var quantidade = _context.Estoques.Where(a => a.ProdutoId == mov.ProdutoId);
                        var total = quantidade.Sum(item => item.Recebido - item.Enviado);

                        if (produtoId)
                        {
                            _context.Produtos.Find(mov.ProdutoId).Quantidade = total;
                        }
                    }
                }
                else
                {
                    pro.Quantidade = 0;
                }
            }
            await _context.SaveChangesAsync();


            var applicationDbContext = _context.Estoques.Include(e => e.Produto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Movimentações
        public async Task<IActionResult> Movimentacoes(Guid id)
        {
            var appDbContext = _context.Estoques.Where(a => a.ProdutoId == id);
            foreach(var x in appDbContext)
            {
                x.Produto = _context.Produtos.Find(x.ProdutoId);
            }
            return View(await appDbContext.ToListAsync());
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
            var quantidadeProdutos = _context.Produtos.Find(estoque.ProdutoId).Quantidade;
            var quantidadeTotal = quantidadeProdutos + estoque.Recebido;
            estoque.Usuario = _userManager.GetUserName(HttpContext.User);
            if (quantidadeTotal >= estoque.Enviado)
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
                return View("_AvisoEstoqueEsgotado");
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

                        estoque.Usuario = _userManager.GetUserName(HttpContext.User);

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
                return View("_AvisoEstoqueEsgotado");

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
