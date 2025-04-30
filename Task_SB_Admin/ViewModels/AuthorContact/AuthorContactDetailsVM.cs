namespace Task_SB_Admin.ViewModels.AuthorContact
{
    public class AuthorContactDetailsVM
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
