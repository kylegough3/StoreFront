using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.Data.EF.Models;
using Microsoft.AspNetCore.Authorization; //used to lock down controller

namespace StoreFront.UI.MVC.Controllers
{

    public class ManufacturersController : Controller
    {
        private readonly Gough_StoreFrontContext _context;

        public ManufacturersController(Gough_StoreFrontContext context)
        {
            _context = context;
        }

        // GET: Manufacturers
        public async Task<IActionResult> Index()
        {
              return _context.Manufacturers != null ? 
                          View(await _context.Manufacturers.ToListAsync()) :
                          Problem("Entity set 'Gough_StoreFrontContext.Manufacturers'  is null.");
        }


        #region Orignal Scaffolded EF Actions
                
        // GET: Manufacturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Manufacturers == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.ManufacturerId == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }

        // GET: Manufacturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManufacturerId,ManufacturerName,ManufacturerCity,ManufacturerState")] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manufacturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);
        }

        // GET: Manufacturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Manufacturers == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ManufacturerId,ManufacturerName,ManufacturerCity,ManufacturerState")] Manufacturer manufacturer)
        {
            if (id != manufacturer.ManufacturerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manufacturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufacturerExists(manufacturer.ManufacturerId))
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
            return View(manufacturer);
        }

        // GET: Manufacturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Manufacturers == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.ManufacturerId == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Manufacturers == null)
            {
                return Problem("Entity set 'Gough_StoreFrontContext.Manufacturers'  is null.");
            }
            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer != null)
            {
                _context.Manufacturers.Remove(manufacturer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region AJAX CRUD Functionality
        [AcceptVerbs("POST")]
        public JsonResult AjaxDelete(int id)
        {
            Manufacturer manufacturer = _context.Manufacturers.Find(id);
            _context.Manufacturers.Remove(manufacturer);
            _context.SaveChanges();

            string confirmMessage = $"Deleted {manufacturer.ManufacturerName} from database";
            return Json(new {id=id, message = confirmMessage});
        }

        public PartialViewResult ManufacturerDetails(int id)
        {
            Manufacturer manufacturer = _context.Manufacturers.Find(id);
            return PartialView(manufacturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxCreate(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();

            return Json(manufacturer);
        }

        #endregion

        private bool ManufacturerExists(int id)
        {
          return (_context.Manufacturers?.Any(e => e.ManufacturerId == id)).GetValueOrDefault();
        }
    }
}
