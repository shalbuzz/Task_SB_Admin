using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_SB_Admin.Data;
using Task_SB_Admin.Models;
using Task_SB_Admin.ViewModels.Publisher;

namespace Task_SB_Admin.Controllers
{
    public class PublishersController : Controller
    {
        private readonly AppDbContext _context;

        public PublishersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var publishers = await _context.Publishers.ToListAsync();

            var vmList = publishers.Select(p => new PublisherVM
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Phone = p.Phone,
                Email = p.Email
            }).ToList();

            return View(vmList);
        }

        public IActionResult Create()
        {
            return View(new PublisherCreateVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublisherCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var publisher = new Publisher
                {
                    Name = vm.Name,
                    Address = vm.Address,
                    Phone = vm.Phone,
                    Email = vm.Email
                    
                };

                _context.Add(publisher);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
                return NotFound();

            var vm = new PublisherEditVM
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                Phone = publisher.Phone,
                Email = publisher.Email


            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PublisherEditVM vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var publisher = await _context.Publishers.FindAsync(id);
                if (publisher == null)
                    return NotFound();

                publisher.Name = vm.Name;
                publisher.Address = vm.Address;
                publisher.Phone = vm.Phone;
                publisher.Email = vm.Email;


                _context.Update(publisher);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Publisher is updated!";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
                return NotFound();

            var vm = new PublisherDetailsVM
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                Phone = publisher.Phone,
                Email = publisher.Email
            };

            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
                return NotFound();

            var vm = new PublisherVM
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                Phone = publisher.Phone,
                Email = publisher.Email
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Publisher is deleted";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}