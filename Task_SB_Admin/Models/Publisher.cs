namespace Task_SB_Admin.Models
{
    public class Publisher : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ICollection<Book> Books { get; set; }
    }
    
    
}
