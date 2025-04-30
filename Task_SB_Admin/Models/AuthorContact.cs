namespace Task_SB_Admin.Models
{
    public class AuthorContact : BaseEntity
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
    
}

