using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_SB_Admin.Data;
using Task_SB_Admin.Models;
using Task_SB_Admin.ViewModels.BookCategory;

namespace Task_SB_Admin.Controllers
{
    public class BookCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public BookCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.BookCategories.ToListAsync();

            var vmList = categories.Select(c => new BookCategoryVM
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return View(vmList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCategoryCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var category = new BookCategory
                {
                    Name = vm.Name
                };

                await _context.BookCategories.AddAsync(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Категория добавлена!";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.BookCategories.FindAsync(id);
            if (category == null) return NotFound();

            var vm = new BookCategoryEditVM
            {
              
                Name = category.Name
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookCategoryEditVM vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var category = await _context.BookCategories.FindAsync(id);
                if (category == null) return NotFound();

                category.Name = vm.Name;

                _context.Update(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Категория обновлена!";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.BookCategories.FindAsync(id);
            if (category == null) return NotFound();

            var vm = new BookCategoryDetailsVM
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.BookCategories.FindAsync(id);
            if (category == null) return NotFound();

            var vm = new BookCategoryVM
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.BookCategories.FindAsync(id);
            if (category != null)
            {
                _context.BookCategories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Категория удалена!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}