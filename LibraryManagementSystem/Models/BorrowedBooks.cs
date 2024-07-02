namespace LibraryManagementSystem.Models
{
    public class BorrowedBooks
    {
        public Guid BarrowedBookId { get; set; }    
        public string? BarrowedBookName { get; set; }
        public string? MemberFullName { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
