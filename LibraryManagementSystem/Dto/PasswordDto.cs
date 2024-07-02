using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dto
{
    public class PasswordDto
    {
        [Required(ErrorMessage = "Parola Sıfırlama Jetonu alanı boş bırakılamaz")]
        public string PasswordResetToken { get; set; }
        [Required(ErrorMessage = "Parola Alanı boş bırakılamaz"), MinLength(6, ErrorMessage = "Şife enaz 6 karater olmalıdır.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Onay Parola alanı boş bırakılamaz") , Compare("Password", ErrorMessage = "1.Parola ve 2.Parola alanları eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }
}
