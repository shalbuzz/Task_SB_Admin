using System.ComponentModel.DataAnnotations;

namespace Task_SB_Admin.ViewModels.BookCategory
{
    public class BookCategoryCreateVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
