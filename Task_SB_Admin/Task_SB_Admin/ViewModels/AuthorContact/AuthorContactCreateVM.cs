using System.ComponentModel.DataAnnotations;

namespace Task_SB_Admin.ViewModels.AuthorContact
{
    public class AuthorContactCreateVM
    {
        [Required]
        public int AuthorId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        public string Address { get; set; }

        public string AuthorName { get; set; }  // для отображения имени автора (можно использовать в SelectList)
    }
}
