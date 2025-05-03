namespace Task_SB_Admin.Models
{
    public class Author : BaseEntity
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }

        public AuthorContact AuthorContact { get; set; }
        public ICollection<BookAuthors> BookAuthors { get; set; }
    }
}
