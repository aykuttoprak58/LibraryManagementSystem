namespace LibraryManagementSystem.Models
{
    public class Members
    {
        public Guid MemberId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string? IdentityNumber { get; set; }
        public string? MemberShipType { get; set; }
        public int MemberShipStatusId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? PasswordResetToken { get; set; }  
        public DateTime ResetTokenExpires { get; set; }     
    }
}
