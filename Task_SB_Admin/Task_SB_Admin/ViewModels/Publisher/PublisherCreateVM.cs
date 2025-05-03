using System.ComponentModel.DataAnnotations;

namespace Task_SB_Admin.ViewModels.Publisher
{
    public class PublisherCreateVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
