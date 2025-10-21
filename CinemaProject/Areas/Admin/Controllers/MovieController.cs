

using CinemaProject.Data;
using CinemaProject.Models;
using CinemaProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly ApplicationDBContext _context;
        private const int PageSize = 8;

        public MovieController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int currentPage = 1)
        {
            var totalMovies = _context.Movies.Count();
            var totalPages = (int)Math.Ceiling(totalMovies / (double)PageSize);

            var movies = _context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Category)
                .AsNoTracking()
                .OrderByDescending(m => m.Id)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewData["CurrentPage"] = currentPage;
            ViewData["TotalPages"] = totalPages;

            return View(movies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new MovieVM
            {
                Movie = new Movie(),
                Categories = new SelectList(_context.Categories.ToList(), "Id", "Name"),
                Cinemas = new SelectList(_context.Cinemas.ToList(), "Id", "Name")
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile? mainImage, List<IFormFile>? file)
        {
            
            if (mainImage != null && mainImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(mainImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Movies", fileName);

              
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var stream = System.IO.File.Create(filePath))
                {
                    mainImage.CopyTo(stream);
                }

                movie.ImgUrl = fileName;
            }

           
            _context.Movies.Add(movie);
            _context.SaveChanges();

         
            if (file != null && file.Count > 0)
            {
                foreach (var item in file)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Movies/SubImages", fileName);

                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        item.CopyTo(stream);
                    }

                }
                _context.SaveChanges();
            }

            TempData["Success"] = "Movie created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
                return RedirectToAction(nameof(Index));

            var viewModel = new MovieVM
            {
                Movie = movie,
                Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", movie.CategoryId),
                Cinemas = new SelectList(_context.Cinemas.ToList(), "Id", "Name", movie.CinemaId)
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile? mainImage, List<IFormFile>? file)
        {
            var movieInDb = _context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == movie.Id);

            if (movieInDb == null)
                return RedirectToAction(nameof(Index));

           
            if (mainImage != null && mainImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(mainImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Movies", fileName);

                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var stream = System.IO.File.Create(filePath))
                {
                    mainImage.CopyTo(stream);
                }

                movie.ImgUrl = fileName;
            }
            else
            {
                movie.ImgUrl = movieInDb.ImgUrl;
            }

           
            if (file != null && file.Count > 0)
            {
                foreach (var item in file)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Movies/SubImages", fileName);

                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        item.CopyTo(stream);
                    }

                  
                }
                _context.SaveChanges();
            }

            _context.Movies.Update(movie);
            _context.SaveChanges();

            TempData["Success"] = "Movie updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
                return RedirectToAction(nameof(Index));

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            TempData["Success"] = "Movie deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}