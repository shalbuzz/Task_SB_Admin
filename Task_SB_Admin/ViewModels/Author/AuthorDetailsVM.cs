namespace Task_SB_Admin.ViewModels.Author
{
    public class AuthorDetailsVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }

        public string AuthorContactEmail { get; set; }  // Данные из AuthorContact
        public string AuthorContactPhone { get; set; }
    }
}
