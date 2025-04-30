using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task_SB_Admin.Data;
using Task_SB_Admin.Models;
using Task_SB_Admin.ViewModels.BookAuthor;

namespace Task_SB_Admin.Controllers
{
    public class BookAuthorsController : Controller
    {
        private readonly AppDbContext _context;

        public BookAuthorsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bookAuthors = await _context.BookAuthors
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .ToListAsync();

            return View(bookAuthors);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new BookAuthorCreateVM
            {
                Books = new SelectList(await _context.Books.ToListAsync(), "Id", "Title"),
                Authors = new SelectList(await _context.Authors.ToListAsync(), "Id", "Name")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookAuthorCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var exists = await _context.BookAuthors
                    .AnyAsync(ba => ba.BookId == vm.BookId && ba.AuthorId == vm.AuthorId);

                if (!exists)
                {
                    var bookAuthor = new BookAuthors
                    {
                        BookId = vm.BookId,
                        AuthorId = vm.AuthorId
                    };

                    _context.BookAuthors.Add(bookAuthor);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Author successfully assigned to the book!";
                }
                else
                {
                    TempData["Error"] = "This author is already assigned to the book.";
                }

                return RedirectToAction(nameof(Index)); 
            }

            vm.Books = new SelectList(await _context.Books.ToListAsync(), "Id", "Title");
            vm.Authors = new SelectList(await _context.Authors.ToListAsync(), "Id", "Name");

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int bookId, int authorId)
        {
            var bookAuthor = await _context.BookAuthors
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);

            if (bookAuthor == null)
                return NotFound();

            return View(bookAuthor);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int bookId, int authorId)
        {
            var bookAuthor = await _context.BookAuthors
                .FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);

            if (bookAuthor == null)
                return NotFound();

            return View(bookAuthor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int bookId, int authorId)
        {
            var bookAuthor = await _context.BookAuthors
                .FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);

            if (bookAuthor != null)
            {
                _context.BookAuthors.Remove(bookAuthor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Author removed from the book.";
            }

            return RedirectToAction(nameof(Index));
        }

        
    }
}
