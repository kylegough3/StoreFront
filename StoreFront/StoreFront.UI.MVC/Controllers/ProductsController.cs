using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using GadgetStore.UI.MVC.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.Data.EF.Models;

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ProductsController : Controller
    {
        private readonly Gough_StoreFrontContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(Gough_StoreFrontContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var gough_StoreFrontContext = _context.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.Size).Include(p => p.StockStatus);
            return View(await gough_StoreFrontContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> TiledProducts(string searchTerm, int categoryId)
        {
            var products = _context.Products
                .Include(p => p.Category).Include (p => p.Manufacturer).Include(p => p.StockStatus).ToList();

            #region Category Search

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type");
            ViewBag.Category = 0;
            if (categoryId != 0)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();
                //Repopulate the dropdown menu with the currently selected category
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type", categoryId);

                ViewBag.Category = categoryId;
            }
            #endregion

            #region Search Filter
            if (!String.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                products = products.Where(p =>
                p.Name.ToLower().Contains(searchTerm) ||
                p.Manufacturer.ManufacturerName.ToLower().Contains(searchTerm) ||
                p.Category.Type.ToLower().Contains(searchTerm) ||
                p.Description.ToLower().Contains(searchTerm))
                    .ToList();

                ViewBag.NbrResults = products.Count;
                ViewBag.SearchTerm = searchTerm;
            }
            else
            {
                ViewBag.NbrResults = null;
                ViewBag.SearchTerm = null;
            }
            #endregion

            return View(products);
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.Size)
                .Include(p => p.StockStatus)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName");
            ViewData["StockStatusId"] = new SelectList(_context.StockStatuses, "StockStatusId", "Status");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,CategoryId,SizeId,Price,ManufacturerId,StockStatusId,QtyInStock,ProductImage,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                #region File Upload - Create
                if (product.Image != null)
                {
                    string ext = Path.GetExtension(product.Image.FileName);

                    string[] validExts = { ".jpg", ".jpeg", ".gif", ".png" };

                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        product.ProductImage = Guid.NewGuid() + ext;

                        string webRootPath = _webHostEnvironment.WebRootPath;

                        string fullImagePath = webRootPath + "/images/";

                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);

                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                    else
                    {
                        product.ProductImage = "noimage.png";
                    }
                }
                #endregion


                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "Manufacturer", product.ManufacturerId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", product.SizeId);
            ViewData["StockStatusId"] = new SelectList(_context.StockStatuses, "StockStatusId", "Status", product.StockStatusId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName");
            ViewData["StockStatusId"] = new SelectList(_context.StockStatuses, "StockStatusId", "Status");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,CategoryId,SizeId,Price,ManufacturerId,StockStatusId,QtyInStock,ProductImage, Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string oldImageName = product.ProductImage;

                if (product.Image != null)
                {
                    string ext = Path.GetExtension(product.Image.FileName);

                    string[] validExts = { ".jpg", "jpeg", ".gif", ".png" };

                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        product.ProductImage = Guid.NewGuid() + ext;

                        string webRootPath = _webHostEnvironment.WebRootPath;

                        string fullPath = webRootPath + "/images/";

                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);

                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullPath,product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }



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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "Manufacturer", product.ManufacturerId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", product.SizeId);
            ViewData["StockStatusId"] = new SelectList(_context.StockStatuses, "StockStatusId", "Status", product.StockStatusId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.Size)
                .Include(p => p.StockStatus)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'Gough_StoreFrontContext.Products'  is null.");
            }
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
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
