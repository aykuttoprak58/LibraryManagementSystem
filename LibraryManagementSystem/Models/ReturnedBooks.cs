namespace LibraryManagementSystem.Models
{
    public class ReturnedBooks
    {
        public Guid ReturnedBookId { get; set; }
        public string? ReturnedBookName { get; set; }
        public string? MemberFullName { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? PunishmentStatus { get; set; }   
    }
}
