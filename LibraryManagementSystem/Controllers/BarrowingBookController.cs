using LibraryManagementSystem.Connetcion;
using LibraryManagementSystemApi.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;

namespace LibraryManagementSystemUI.Controllers
{
    public class BarrowingBookController : Controller
    {
        Uri baseAddrress = new Uri("https://localhost:44360/api");
        private readonly HttpClient _httpClient;

        public BarrowingBookController()
        {
            _httpClient = new HttpClient();

            _httpClient.BaseAddress = baseAddrress;
        }

        [HttpGet]
        public IActionResult GetAllBarrowedBooksUI(BorrowedBookDto model)
        {
            string MemberId = HttpContext.Session.GetString("MemberId");
            string IdentityNumber = HttpContext.Session.GetString("IdentityNumber");
            string MemberShipType = HttpContext.Session.GetString("MemberShipType");
            string Username = HttpContext.Session.GetString("Username");
            string Name = HttpContext.Session.GetString("Name");
            string Surname = HttpContext.Session.GetString("Surname");
            TempData["role"] = MemberShipType;
            TempData["username"] = Name + " " + Surname;
            var myModel = MyQuery.GetAllBorrowedBooks(MemberShipType, IdentityNumber);

            return View(myModel);
        }

        [HttpPost]
        public IActionResult CreateBarrowedBookUI(Guid bookId)
        {
            var books = MyQuery.GetBookNameByBookId(bookId);
            if (books == "There is no book with this number")
            {
                return BadRequest("There is no book with this number");
            }
            var currentTime = DateTime.Now;
            var closingTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 18, 0, 0);
            var BorrowedDate = DateTime.Now;
            var ReturnDate = DateTime.Now;
            var bookName = MyQuery.GetBookNameByBookId(bookId);
            if (currentTime > closingTime)
            {
                return BadRequest("The Library is closed");
            }

            string userName = HttpContext.Session.GetString("UserName");
            string name = HttpContext.Session.GetString("Name");
            string surname = HttpContext.Session.GetString("Surname");
            string identityNumber = HttpContext.Session.GetString("IdentityNumber");
            string memberId = HttpContext.Session.GetString("MemberId");
            string memberShipType = HttpContext.Session.GetString("MemberShipType");

            if (memberShipType == "Student")
            {
                ReturnDate = DateTime.Now.AddDays(30);
            }

            else if (memberShipType == "Citizen")
            {
                ReturnDate = DateTime.Now.AddDays(14);
            }

            else if (memberShipType == "Admin")
            {
                ReturnDate = DateTime.Now.AddDays(14);

            }

            var bookStatus = MyQuery.BookStatusCheck(bookId);

            var bbnNumber = MyQuery.BBNCheck(identityNumber);

            if (bookStatus == "Ödünç Alındı" && bookStatus != "Rafta")
            {
                TempData["Message"] = "Kitap ödünç alınmış, başka bir kitap deneyin.";
                return RedirectToAction("GetAllBooksUI","Book");
            }

            if (memberShipType == "Citizen")
            {
                if (bbnNumber == 2)
                {
                    TempData["Message"] = "Kitap ödünç alma limitini aştınız. En fazla iki kitap alma hakkınız bulunmaktadır.";
                    return RedirectToAction("GetAllBooksUI", "Book");
                }
            }

            if (memberShipType == "Student")
            {
                if (bbnNumber == 4)
                {
                    TempData["Message"] = "Kitap ödünç alma limitini aştınız. En fazla dört kitap alma hakkınız bulunmaktadır.";
                    return RedirectToAction("GetAllBooksUI", "Book");
                }
            }


            if (userName != "" && name != "" && surname != "" && identityNumber != "")
            {
                using (SqlConnection con = new SqlConnection(MyQuery.GetConnectionString()))
                {
                    con.Open();
                    string insertQuery = "INSERT INTO BorrowedBooks (BarrowedBookId, BarrowedBookName, MemberFullName, IdentityNumber, BorrowedDate, ReturnDate) VALUES (@BarrowedBookId, @BarrowedBookName, @MemberFullName, @IdentityNumber, @BorrowedDate, @ReturnDate)";
                    using (SqlCommand com = new SqlCommand())
                    {
                        com.CommandText = insertQuery;
                        com.Connection = con;
                        com.Parameters.AddWithValue("@BarrowedBookId", bookId);
                        com.Parameters.AddWithValue("@BarrowedBookName", bookName);
                        com.Parameters.AddWithValue("@MemberFullName", name + " " + surname);
                        com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                        com.Parameters.AddWithValue("@BorrowedDate", BorrowedDate);
                        com.Parameters.AddWithValue("@ReturnDate", ReturnDate);
                        var affectedRow = com.ExecuteNonQuery();
                        MyQuery.UpdateBookStatus(Convert.ToString(bookId), "Ödünç Alındı");
                        MyQuery.UpdateBBNPlus(identityNumber);
                        return RedirectToAction("GetAllBarrowedBooksUI");   
                    }
                }

            }
            return RedirectToAction("GetAllBarrowedBooksUI");
        }
    }
}
