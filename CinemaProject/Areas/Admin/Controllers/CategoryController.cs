using CinemaProject.Data;
using CinemaProject.Models;
using CinemaProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MoviesDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _context;
        private const int PageSize = 8;

        public CategoryController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int currentPage = 1)
        {
            var totalCategories = _context.Categories.Count();
            var totalPages = (int)Math.Ceiling(totalCategories / (double)PageSize);
            var categories = _context.Categories
                .AsNoTracking()
                .OrderByDescending(c => c.Id)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            ViewData["CurrentPage"] = currentPage;
            ViewData["TotalPages"] = totalPages;
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var category = new Category();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]  
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        [HttpPost]
        
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);
            var oldCategory = _context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == category.Id);
            if (oldCategory == null)
                return NotFound();

            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]  
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}