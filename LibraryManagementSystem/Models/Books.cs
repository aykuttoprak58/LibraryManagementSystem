using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Books
    {
        [Required]
        [DisplayName("Kitap Id")]
        public Guid BookId { get; set; }
        [Required]
        [DisplayName("Kitap Adısdasda")]
        public string? BookName { get; set; }
        [Required]
        [DisplayName("Yazar Adı")]
        public int AuthorId { get; set; }
        [Required]
        [DisplayName("Kategori Adı")]
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Yayın Tarihi")]
        public int ReleaseDate { get; set; }
        [Required]
        [DisplayName("Kitap Durumu")]
        public string? BookStatus { get; set; }
    }
}
