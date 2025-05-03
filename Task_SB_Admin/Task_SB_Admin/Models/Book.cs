using Task_SB_Admin.ViewModels.BookAuthor;

namespace Task_SB_Admin.Models
{
        public class Book : BaseEntity
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public int PublicationYear { get; set; }
            public decimal Price { get; set; }

            public int BookCategoryId { get; set; }
            public BookCategory BookCategory { get; set; }
        
            public int PublisherId { get; set; }
            public Publisher Publisher { get; set; }

            public ICollection<BookAuthors> BookAuthors { get; set; } = new List<BookAuthors>();
    }
}
