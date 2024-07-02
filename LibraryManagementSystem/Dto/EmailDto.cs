using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemApi.Dto
{
    public class EmailDto
    {
        [Required(ErrorMessage = "Email alanı boş bırakılamaz")]
        public string Email { get; set; }   
    }
}
