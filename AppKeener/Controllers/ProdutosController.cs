using System;
using System.IO;
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
using AppKeener.ViewModels;

namespace AppKeener.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;        
        private readonly ApplicationDbContext _context;               
        public ProdutosController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
            _context = context;            
        }

        // GET: Produtos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {            
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
        public async Task<IActionResult> Create(ProdutoViewModel model)
        {
            if (ModelState.IsValid)
            {  
                if(!(_context.Produtos.FirstOrDefault(a=>a.Nome == model.Nome) != null)){
                   

                    string uniqueFileName = UploadedFile(model);

                    Produto produto = new Produto
                    {
                        Nome = model.Nome,
                        Descricao = model.Descricao,
                        Id = Guid.NewGuid(),
                        Quantidade = 0,
                        DataCadastro = model.DataCadastro,
                        Valor = model.Valor,                        
                        Imagem = uniqueFileName,
                    };

                    _context.Add(produto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } 
                else
                {
                    Console.Beep();
                    return View("_AvisoProdutoCadastrado");

                }
            }

            return View();
        }

        private string UploadedFile(ProdutoViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
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

        //GET: Movimentações
        public IActionResult Movimentacoes(Guid id)
        {
            return RedirectToAction("Movimentacoes", "Estoque", new { @id = id });
        }
    }
}
