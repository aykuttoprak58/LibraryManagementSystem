namespace LibraryManagementSystem.Models
{
    public class BookListViewModel
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<BookViewModel> Books { get; set; }
    }
}
