namespace LibraryManagementSystemApi.Models
{
    public class Settings
    {
        public int SettingId { get; set; }      
        public string SettingName { get; set; } = string.Empty;     
        public bool SettingStatus { get; set; } 
    }
}
