using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Task_SB_Admin.Data;
using Task_SB_Admin.Models;
using Task_SB_Admin.ViewModels.Book;
using Task_SB_Admin.ViewModels.BookAuthor;
using Task_SB_Admin.ViewModels.Books;

namespace Task_SB_Admin.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
      .Include(b => b.BookCategory)
      .Include(b => b.Publisher)
      .Include(b => b.BookAuthors)
          .ThenInclude(ba => ba.Author)
      .ToListAsync();


            var vmList = books.Select(b => new BookVM
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                PublicationYear = b.PublicationYear,
                BookCategoryName = b.BookCategory.Name,
                PublisherName = b.Publisher.Name,
                Authors = b.BookAuthors.Select(ba => ba.Author.FullName).ToList(),
                Price = b.Price
            }).ToList();

            return View(vmList);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new BookCreateVM
            {
                
                BookCategories = new SelectList(await _context.BookCategories.ToListAsync(), "Id", "Name"),
                Publishers = new SelectList(await _context.Publishers.ToListAsync(), "Id", "Name"),
 
                Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName")
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.BookCategories = new SelectList(await _context.BookCategories.ToListAsync(), "Id", "Name");
                vm.Publishers = new SelectList(await _context.Publishers.ToListAsync(), "Id", "Name");
                vm.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName");

                return View(vm);
            }

            var book = new Book
            {
                Title = vm.Title,
                Description = vm.Description,
                PublicationYear = vm.PublicationYear,
                Price = vm.Price,
                BookCategoryId = vm.BookCategoryId,
                PublisherId = vm.PublisherId,
                BookAuthors = vm.AuthorIds?.Select(authorId => new BookAuthors
                {
                    AuthorId = authorId
                }).ToList() ?? new List<BookAuthors>()

            };

            

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Book added successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            var vm = new BookEditVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublicationYear = book.PublicationYear,
                BookCategoryId = book.BookCategoryId,
                PublisherId = book.PublisherId,
                Price = book.Price,
                AuthorIds = book.BookAuthors.Select(ba => ba.AuthorId).ToList(),
                BookCategories = new SelectList(await _context.BookCategories.ToListAsync(), "Id", "Name", book.BookCategoryId),
                Publishers = new SelectList(await _context.Publishers.ToListAsync(), "Id", "Name", book.PublisherId),
                Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName", book.BookAuthors.Select(ba => ba.AuthorId))
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookEditVM vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.BookCategories = new SelectList(await _context.BookCategories.ToListAsync(), "Id", "Name", vm.BookCategoryId);
                vm.Publishers = new SelectList(await _context.Publishers.ToListAsync(), "Id", "Name", vm.PublisherId);
                vm.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName", vm.AuthorIds);
                return View(vm);
            }

            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            book.Title = vm.Title;
            book.Description = vm.Description;
            book.PublicationYear = vm.PublicationYear;
            book.BookCategoryId = vm.BookCategoryId;
            book.PublisherId = vm.PublisherId;
            book.Price = vm.Price;

          
            book.BookAuthors.Clear();
            if (vm.AuthorIds != null && vm.AuthorIds.Any())
            {
                book.BookAuthors = vm.AuthorIds.Select(authorId => new BookAuthors
                {
                    BookId = book.Id,
                    AuthorId = authorId
                }).ToList();
            }

            _context.Update(book);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Book updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookCategory)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            var vm = new BookDetailsVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublicationYear = book.PublicationYear,
                BookCategoryName = book.BookCategory.Name,
                PublisherName = book.Publisher.Name,
                Price = book.Price
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookCategory)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            var vm = new BookVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublicationYear = book.PublicationYear,
                BookCategoryName = book.BookCategory?.Name,
                PublisherName = book.Publisher?.Name,
                Price = book.Price
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Book deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}