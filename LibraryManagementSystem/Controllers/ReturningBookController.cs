using LibraryManagementSystem.Connetcion;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace LibraryManagementSystemUI.Controllers
{
    public class ReturningBookController : Controller
    {
        private readonly IConfiguration _configuration;
        public ReturningBookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetAllReturnedBooksUI()
        {
            string MemberId = HttpContext.Session.GetString("MemberId");
            string IdentityNumber = HttpContext.Session.GetString("IdentityNumber");
            string MemberShipType = HttpContext.Session.GetString("MemberShipType");
            string Username = HttpContext.Session.GetString("Username");
            string Name = HttpContext.Session.GetString("Name");
            string Surname = HttpContext.Session.GetString("Surname");
            TempData["username"] = Name + " " + Surname;
            TempData["role"] = MemberShipType;
            var myModel = MyQuery.GetAllReturnedBooks(MemberShipType,IdentityNumber);
            return View(myModel);   
        }

        [HttpPost]
        public IActionResult CreateReturnedBookUI(Guid bookId) 
        {
            var bookName = MyQuery.GetBookNameByBookId(bookId);
            var currentTime = DateTime.Now;
            var closingTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 18, 0, 0);
            var returnDate = MyQuery.CheckBarrowBookDate(bookId);
            string name = HttpContext.Session.GetString("Name");
            string surname = HttpContext.Session.GetString("Surname");
            string identityNumber = HttpContext.Session.GetString("IdentityNumber");
            string memberId = HttpContext.Session.GetString("MemberId");
            string MemberShipType = HttpContext.Session.GetString("MemberShipType");
            Guid guid = Guid.NewGuid();

            var bookStatus = MyQuery.BookStatusCheck(bookId);

            if (bookStatus != "Ödünç Alındı" && bookStatus == "Rafta")
            {
                return BadRequest("The book is in library.It is already returned.");
            }

            if (currentTime > closingTime)
            {
                return BadRequest("The Library is closed");
            }

            if (identityNumber == null)
            {
                return BadRequest("User Not Found");
            }

            if (MemberShipType != "Admin") 
            {
                var identityNumberCheck = MyQuery.identityNumberCheck(identityNumber, bookId);

                if (identityNumberCheck == "0" || identityNumber == null)
                {
                    return BadRequest("User Not Found who borrowed this book.");
                }
            }

            string connString = _configuration.GetConnectionString("MyDbConnection");

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string insertQuery = "Insert into ReturnedBooks (ReturnedBookId,ReturnedBookName,MemberFullName,IdentityNumber,Deadline,ReturnDate,PunishmentStatus) " +
                    "Values (@ReturnedBookId,@ReturnedBookName,@MemberFullName,@IdentityNumber,@Deadline,@ReturnDate,@PunishmentStatus)";
                using (SqlCommand com = new SqlCommand(insertQuery, con))
                {
                    com.Parameters.AddWithValue("@ReturnedBookId", guid);
                    com.Parameters.AddWithValue("@ReturnedBookName", bookName);
                    com.Parameters.AddWithValue("@MemberFullName", name + " " + surname);
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    com.Parameters.AddWithValue("@Deadline", returnDate);
                    com.Parameters.AddWithValue("@ReturnDate", DateTime.Now);

                    if (DateTime.Now.ToShortDateString() == returnDate.ToShortDateString() || DateTime.Now < returnDate)
                    {
                        com.Parameters.AddWithValue("@PunishmentStatus", "Zamanında Teslim Edildi");
                    }

                    else
                    {
                        TimeSpan timePassed = DateTime.Now.Subtract(returnDate);
                        int punishmentNumber = timePassed.Days;
                        com.Parameters.AddWithValue("@PunishmentStatus", $"{punishmentNumber} gün geç iade edildi.");
                        MyQuery.UpdatePunishmentNumberPlus(identityNumber);
                    }

                    var rowAffected = com.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        MyQuery.UpdateBBNMinus(identityNumber);
                        MyQuery.UpdateBookStatus(Convert.ToString(bookId), "Rafta");
                        MyQuery.deleteBarrowedBook(bookId);
                        return RedirectToAction("GetAllReturnedBooksUI");   
                    }

                    else
                    {
                        return BadRequest("Failed to Create");
                    }
                }
            }
        }
    }
}
