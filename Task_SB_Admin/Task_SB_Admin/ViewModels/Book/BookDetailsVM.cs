namespace Task_SB_Admin.ViewModels.Book
{
    public class BookDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PublicationYear { get; set; }
        public decimal Price { get; set; }
        public string BookCategoryName { get; set; } 
        public string PublisherName { get; set; } 

        public List<string> Authors { get; set; } 
    }
}
