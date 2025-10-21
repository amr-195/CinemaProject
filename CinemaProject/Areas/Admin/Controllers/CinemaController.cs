using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaProject.Data;
using CinemaProject.Models;

namespace CinemaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CinemaController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int currentPage = 1)
        {
            const int pageSize = 8;
            var totalCinemas = _context.Cinemas.Count();
            var totalPages = (int)Math.Ceiling(totalCinemas / (double)pageSize);

            var cinemas = _context.Cinemas
                .AsNoTracking()
                .OrderByDescending(c => c.Id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["CurrentPage"] = currentPage;
            ViewData["TotalPages"] = totalPages;

            return View(cinemas);
        }

     
        public IActionResult Create()
        {
            return View(new Cinema());
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cinema cinema, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return View(cinema);

            if (image != null && image.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Cinemas", fileName);

               
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using var stream = System.IO.File.Create(path);
                image.CopyTo(stream);

                cinema.CinemaLogo = fileName;
            }

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

      
        public IActionResult Edit(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null)
                return NotFound();

            return View(cinema);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cinema cinema, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return View(cinema);

            var oldCinema = _context.Cinemas.AsNoTracking().FirstOrDefault(c => c.Id == cinema.Id);
            if (oldCinema == null)
                return NotFound();

            if (image != null && image.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Cinemas", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using var stream = System.IO.File.Create(path);
                image.CopyTo(stream);

                cinema.CinemaLogo = fileName;
            }
            else
            {
                cinema.CinemaLogo = oldCinema.CinemaLogo;
            }

            _context.Cinemas.Update(cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema != null)
            {
                _context.Cinemas.Remove(cinema);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
