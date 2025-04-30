using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Task_SB_Admin.ViewModels.Books
{
    public class BookCreateVM
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
            
        [Required]
        public int PublicationYear { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        public int BookCategoryId { get; set; }

        public SelectList BookCategories { get; set; }

        [Required]
        public int PublisherId { get; set; }

        public SelectList Publishers { get; set; }

        public List<int> AuthorIds { get; set; } = new List<int>();

        public MultiSelectList Authors { get; set; } 
    }
}
