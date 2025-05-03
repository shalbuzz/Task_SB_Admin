using Microsoft.EntityFrameworkCore;
using Task_SB_Admin.Models;

namespace Task_SB_Admin.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorContact> AuthorContacts { get; set; }
        public DbSet<BookAuthors> BookAuthors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }


    }
    
    }

