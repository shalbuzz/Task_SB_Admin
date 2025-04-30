using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task_SB_Admin.Data;
using Task_SB_Admin.Models;
using Task_SB_Admin.ViewModels.AuthorContact;

namespace Task_SB_Admin.Controllers
{
    public class AuthorContactsController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorContactsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _context.AuthorContacts.Include(ac => ac.Author).ToListAsync();

            var contactVMs = contacts.Select(c => new AuthorContactVM
            {
                Id = c.Id,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address,
                AuthorName = c.Author.FullName,
                AuthorId = c.AuthorId
            }).ToList();

            return View(contactVMs);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorContactCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var authorExists = await _context.Authors
                                                 .AnyAsync(a => a.Id == vm.AuthorId);

                if (!authorExists)
                {
                    ModelState.AddModelError(string.Empty, "Author not found.");
                    ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", vm.AuthorId);
                    return View(vm);
                }

                var entity = new AuthorContact
                {
                    Phone = vm.Phone,
                    Email = vm.Email,
                    Address = vm.Address,
                    AuthorId = vm.AuthorId
                };

                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Контакт успешно добавлен!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", vm.AuthorId);
            return View(vm);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _context.AuthorContacts.FindAsync(id);
            if (contact == null) return NotFound();

            var vm = new AuthorContactEditVM
            {
                Id = contact.Id,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,

            };

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", contact.AuthorId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorContactEditVM vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var contact = await _context.AuthorContacts.FindAsync(id);
                if (contact == null) return NotFound();

                contact.Phone = vm.Phone;
                contact.Email = vm.Email;
                contact.Address = vm.Address;
               

                _context.Update(contact);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Контакт обновлён!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", vm.AuthorId);
            return View(vm);
        }


        public async Task<IActionResult> Details(int id)
        {
            var contact = await _context.AuthorContacts
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null) return NotFound();

            var vm = new AuthorContactDetailsVM
            {
                Id = contact.Id,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                AuthorName = contact.Author.FullName,
                AuthorId = contact.AuthorId

            };

            return View(vm);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.AuthorContacts
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null) return NotFound();

            var vm = new AuthorContactVM
            {
                Id = contact.Id,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                AuthorName = contact.Author.FullName,
                AuthorId = contact.AuthorId
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.AuthorContacts.FindAsync(id);
            if (contact == null) return NotFound();

            _context.AuthorContacts.Remove(contact);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Контакт удалён!";
            return RedirectToAction(nameof(Index));
        }

    }
}
