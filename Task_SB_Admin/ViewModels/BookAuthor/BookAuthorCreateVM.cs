using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Task_SB_Admin.ViewModels.BookAuthor
{
    public class BookAuthorCreateVM
    {
        [Required]
        public int BookId { get; set; }

        public SelectList Books { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public SelectList Authors { get; set; }
    }
}
