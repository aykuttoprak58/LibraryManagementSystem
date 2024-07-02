using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LibraryManagementSystem.Dto
{
    public class UserDto
    {
        [Required]
        public Guid MemberId { get; set; }

        [Required (ErrorMessage = "Ad alanı boş bıarakılamaz")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı boş bıarakılamaz")]
        public string Surname { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Cinsiyet alanı boş bıarakılamaz")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Üyelik Türü alanı boş bıarakılamaz")]
        public string MemberShipType { get; set; }

        [Required(ErrorMessage ="TC Kimlik alanı boş bırakılamaz"),StringLength(11, ErrorMessage = "TC Kimlik numarası 11 rakam olmalıdır", MinimumLength = 11)]

        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı alanı boş bıarakılamaz")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Email alanı boş bırakılamaz"), EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre alanı boş bırakılamaz"), MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; }

    }
}
