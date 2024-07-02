using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class BookViewModel
    {
        [Required]
        [DisplayName("Kitap Id")]
        public Guid BookId { get; set; }

        [Required]
        [DisplayName("Kitap Adı")]
        public string? BookName { get; set; }

        [DisplayName("Yazar Adı")]
        public string? AuthorName { get; set; }

        [DisplayName("Kategori Adı")]
        public string? CategoryName { get; set; }

        [Required]
        [DisplayName("Yayın Tarihi")]
        public int ReleaseDate { get; set; }

        [Required]
        [DisplayName("Kitap Durumu")]
        public string? BookStatus { get; set; }
    }
}
