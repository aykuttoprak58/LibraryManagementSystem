namespace LibraryManagementSystem.Models
{
    public class MemberStatus
    {
        public Guid MemberId { get; set; }  
        public string? MemberFullName { get; set; } 
        public string? IdentityNumber { get; set; } 
        public int BarrowedBookNumber { get; set; } 
        public int ReturnedBookNumber { get; set; }     
        public int PunishmentNumber { get; set; }  
        public string? PunishmentStatus { get; set; }    
        public bool PunishmentMailStatus { get; set; }       
    }
}
