using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dto
{
    public class SignInDto
    {

        [Required(ErrorMessage = "Kullanıcı Adı alanı boş bırakılamaz")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        public string? Password { get; set; }
    }
}
