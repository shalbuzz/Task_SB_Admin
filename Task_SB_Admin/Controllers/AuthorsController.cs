using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_SB_Admin.Data;
using Task_SB_Admin.Models;
using Task_SB_Admin.ViewModels.Author;
using Task_SB_Admin.ViewModels.Authors;

namespace Task_SB_Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors.Include(a => a.AuthorContact).ToListAsync();

            var authorVMs = authors.Select(a => new AuthorVM
            {
                Id = a.Id,
                FullName = a.FullName,
                DateOfBirth = a.DateOfBirth,
                Biography = a.Biography,
                AuthorContactPhone = a.AuthorContact?.Phone,
                AuthorContactEmail = a.AuthorContact?.Email
            }).ToList();

            return View(authorVMs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var author = new Author
                {

                    FullName = vm.FullName,
                    DateOfBirth = vm.DateOfBirth,
                    Biography = vm.Biography
                };

                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Автор добавлен!";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            var vm = new AuthorEditVM
            {
                Id = author.Id,
                FullName = author.FullName,
                DateOfBirth = author.DateOfBirth,
                Biography = author.Biography
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorEditVM vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null) return NotFound();

                author.FullName = vm.FullName;
                author.DateOfBirth = vm.DateOfBirth;
                author.Biography = vm.Biography;

                _context.Update(author);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Автор обновлён!";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            var vm = new AuthorDetailsVM
            {
                Id = author.Id,
                FullName = author.FullName,
                DateOfBirth = author.DateOfBirth,
                Biography = author.Biography,
                AuthorContactEmail = author.AuthorContact?.Email,
                AuthorContactPhone = author.AuthorContact?.Phone
            };

            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            var vm = new AuthorVM
            {
                Id = author.Id,
                FullName = author.FullName,
                DateOfBirth = author.DateOfBirth,
                Biography = author.Biography
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Автор удалён!";
            return RedirectToAction(nameof(Index));
        }
    }
}