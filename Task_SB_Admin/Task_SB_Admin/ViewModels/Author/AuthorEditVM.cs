using System.ComponentModel.DataAnnotations;

namespace Task_SB_Admin.ViewModels.Author
{
    public class AuthorEditVM
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string Biography { get; set; }

        [EmailAddress]
        public string AuthorContactEmail { get; set; }

        [Required, Phone]
        public string AuthorContactPhone { get; set; }
    }
}
