using CinemaProject.Data;
using CinemaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaProject.Areas.Admin.Controllers
{
   
        [Area("Admin")]
        public class ActorController : Controller
        {
            private readonly ApplicationDBContext _context;
            private const int PageSize = 8;

            public ActorController(ApplicationDBContext context)
            {
                _context = context;
            }

          
            public IActionResult Index(int currentPage = 1)
            {
                var totalActors = _context.Actors.Count();
                var totalPages = (int)Math.Ceiling(totalActors / (double)PageSize);

                var actors = _context.Actors
                    .AsNoTracking()
                    .OrderByDescending(a => a.Id)
                    .Skip((currentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                ViewData["CurrentPage"] = currentPage;
                ViewData["TotalPages"] = totalPages;

                return View(actors);
            }

          
            [HttpGet]
            public IActionResult Create()
            {
                return View(new Actor());
            }

            [HttpPost]
           // [ValidateAntiForgeryToken]
            public IActionResult Create(Actor actor, IFormFile img)
            {
                if (!ModelState.IsValid)
                    return View(actor);

                string fileName = null!;
                if (img is not null && img.Length > 0)
                {
                    fileName = $"{Guid.NewGuid()}{Path.GetExtension(img.FileName)}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Actors", fileName);

                    using var stream = System.IO.File.Create(filePath);
                    img.CopyTo(stream);
                }

                actor.ProfilePicture = fileName;
                _context.Actors.Add(actor);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

           
            [HttpGet]
            public IActionResult Edit(int id)
            {
                var actor = _context.Actors.Find(id);
                if (actor == null)
                    return NotFound();

                return View(actor);
            }

            [HttpPost]
           // [ValidateAntiForgeryToken]
            public IActionResult Edit(Actor actor, IFormFile? img)
            {
                if (!ModelState.IsValid)
                    return View(actor);

                var oldActor = _context.Actors.AsNoTracking().FirstOrDefault(a => a.Id == actor.Id);
                if (oldActor == null)
                    return NotFound();

                string fileName = oldActor.ProfilePicture ?? "";

                if (img is not null && img.Length > 0)
                {
                    fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(img.FileName)}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Actors", fileName);

                    using var stream = System.IO.File.Create(filePath);
                    img.CopyTo(stream);
                }

                actor.ProfilePicture= fileName;

                _context.Actors.Update(actor);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            
            [HttpGet]
            public IActionResult Delete(int id)
            {
                var actor = _context.Actors.Find(id);
                if (actor == null)
                    return NotFound();

                _context.Actors.Remove(actor);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
        }
    }
