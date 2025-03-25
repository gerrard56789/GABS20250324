using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GABS2025324AppWebMVC.Models;

namespace GABS2025324AppWebMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly Test20250324DbContext _context;

        public ProductController(Test20250324DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Product Product, int topRegistro = 10)
        {
            try
            {
                var query = _context.Products.AsQueryable();
                if (!string.IsNullOrWhiteSpace(Product.ProductName))
                    query = query.Where(p => p.ProductName.Contains(Product.ProductName));

                if (!string.IsNullOrWhiteSpace(Product.Description))
                    query = query.Where(p => p.Description.Contains(Product.Description));

                if (Product.BrandId > 0)
                    query = query.Where(p => p.BrandId == Product.BrandId);

                if (Product.WarehouseId > 0)
                    query = query.Where(p => p.WarehouseId == Product.WarehouseId);

                query = query.Take(topRegistro);


                query = query.Include(p => p.WarehouseId)
                             .Include(p => p.Brand);


                var productos = await query.ToListAsync();


                var marcas = _context.Brands.ToList();
                marcas.Insert(0, new Brand { BrandName = "SELECCIONAR", BrandsId = 0 });

                var categorias = _context.Warehouses.ToList();
                categorias.Insert(0, new Warehouse { WarehouseName = "SELECCIONAR", WarehouseId = 0 });

                ViewData["WarehouseId"] = new SelectList(categorias, "WarehouseId", "WarehouseName", 0);
                ViewData["BrandId"] = new SelectList(marcas, "BrandId", "BrandName", 0);

                return View(productos);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = "Ha ocurrido un error al cargar los productos.";
                return View(new List<Product>());
            }
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandsId", "BrandsId");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "WarehouseId");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Description,Price,PurchasePrice,WarehouseId,BrandId,Notes")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandsId", "BrandsId", product.BrandId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "WarehouseId", product.WarehouseId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandsId", "BrandsId", product.BrandId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "WarehouseId", product.WarehouseId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,Price,PurchasePrice,WarehouseId,BrandId,Notes")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandsId", "BrandsId", product.BrandId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "WarehouseId", product.WarehouseId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
