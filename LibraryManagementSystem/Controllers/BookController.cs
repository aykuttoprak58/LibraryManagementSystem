using LibraryManagementSystem.Connetcion;
using LibraryManagementSystem.Models;
using LibraryManagementSystemApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace LibraryManagementSystemUI.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;


        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
       
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllBooksUI(int pageNumber = 1, int pageSize = 5, string searchTerm = "")
        {
            string Name = HttpContext.Session.GetString("Name");
            string Surname = HttpContext.Session.GetString("Surname");
            TempData["username"] = Name + " " + Surname;
            string MemberShipType = HttpContext.Session.GetString("MemberShipType");
            TempData["role"] = MemberShipType;
            if (TempData["username"] == null || TempData["role"] == null)
            {
                return RedirectToAction("SignInUI", "Auth");
            }

            string connString = _configuration.GetConnectionString("MyDbConnection");

            int totalCount = 0;
            int totalPages = 0;
            List<BookViewModel> books = new List<BookViewModel>();

            string countQuery = "SELECT COUNT(*) FROM Books";

            using (SqlConnection countConnection = new SqlConnection(connString))
            {
                countConnection.Open();
                using (SqlCommand countCommand = new SqlCommand(countQuery, countConnection))
                {
                    totalCount = (int)countCommand.ExecuteScalar();
                }
            }

            totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            searchTerm = searchTerm.Trim(); // Ön ve son boşlukları kaldır

            string[] searchTerms = searchTerm.Split(' '); // Kelimeleri ayır
            string likeCondition = string.Join(" AND ", searchTerms.Select(term => $"(b.BookName LIKE '%{term}%' OR a.AuthorName LIKE '%{term}%' OR c.CategoryName LIKE '%{term}%')"));
            string query = $@"SELECT b.BookId, b.BookName, b.ReleaseDate, b.BookStatus,
                a.AuthorName, c.CategoryName FROM Books b 
                INNER JOIN Authors a ON b.AuthorId = a.AuthorId
                INNER JOIN Categories c ON b.CategoryId = c.CategoryId
                WHERE {likeCondition}
                ORDER BY b.BookId
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";


            int offset = (pageNumber - 1) * pageSize;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookViewModel book = new BookViewModel
                            {
                                BookId = (Guid)reader["BookId"],
                                BookName = reader["BookName"].ToString(),
                                AuthorName = reader["AuthorName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                ReleaseDate = (int)reader["ReleaseDate"],
                                BookStatus = reader["BookStatus"].ToString()
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            if (books.Count > 0)
            {
                var model = new BookListViewModel
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    Books = books
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("GetAllBooksUI");
            }


            //string connString = _configuration.GetConnectionString("MyDbConnection");

            //string query = @"SELECT b.BookId, b.BookName, b.ReleaseDate, b.BookStatus,
            //         a.AuthorName, c.CategoryName FROM Books b 
            //         INNER JOIN Authors a ON b.AuthorId = a.AuthorId
            //         INNER JOIN Categories c ON b.CategoryId = c.CategoryId";

            //List<BookViewModel> books = new List<BookViewModel>();

            //using (SqlConnection connection = new SqlConnection(connString))
            //{
            //    connection.Open();
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                BookViewModel book = new BookViewModel
            //                {
            //                    BookId = (Guid)reader["BookId"],
            //                    BookName = reader["BookName"].ToString(),
            //                    AuthorName = reader["AuthorName"].ToString(),
            //                    CategoryName = reader["CategoryName"].ToString(),
            //                    ReleaseDate = (int)reader["ReleaseDate"],
            //                    BookStatus = reader["BookStatus"].ToString()
            //                };
            //                books.Add(book);
            //            }
            //        }
            //    }
            //}

            //if (books.Count > 0)
            //{
            //    return View(books);
            //}
            //else
            //{
            //    return NotFound();
            //}

            //var bookList = new List<BookListViewModel>();
            //HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Book/GetAllBooks").Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    string data = response.Content.ReadAsStringAsync().Result;
            //    bookList = JsonConvert.DeserializeObject<List<BookListViewModel>>(data);

            //}

            //return View(bookList);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllBooksUI9(int pageNumber = 1, int pageSize = 10, string searchTerm = "")
        {
            string Name = HttpContext.Session.GetString("Name");
            string Surname = HttpContext.Session.GetString("Surname");
            TempData["username"] = Name + " " + Surname;
            string MemberShipType = HttpContext.Session.GetString("MemberShipType");
            TempData["role"] = MemberShipType;
            if (TempData["username"] == null || TempData["role"] == null)
            {
                return RedirectToAction("SignInUI", "Auth");
            }

            string connString = _configuration.GetConnectionString("MyDbConnection");

            int totalCount = 0;
            int totalPages = 0;
            List<BookViewModel> books = new List<BookViewModel>();

            string countQuery = "SELECT COUNT(*) FROM Books";

            using (SqlConnection countConnection = new SqlConnection(connString))
            {
                countConnection.Open();
                using (SqlCommand countCommand = new SqlCommand(countQuery, countConnection))
                {
                    totalCount = (int)countCommand.ExecuteScalar();
                }
            }

            totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            searchTerm = searchTerm.Replace(" ", "");
            string query = @"SELECT b.BookId, b.BookName, b.ReleaseDate, b.BookStatus,
                     a.AuthorName, c.CategoryName FROM Books b 
                     INNER JOIN Authors a ON b.AuthorId = a.AuthorId
                     INNER JOIN Categories c ON b.CategoryId = c.CategoryId
                     WHERE b.BookName LIKE @SearchTerm OR a.AuthorName LIKE @SearchTerm
                     OR c.CategoryName LIKE @SearchTerm
                     ORDER BY b.BookId
                     OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            int offset = (pageNumber - 1) * pageSize;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookViewModel book = new BookViewModel
                            {
                                BookId = (Guid)reader["BookId"],
                                BookName = reader["BookName"].ToString(),
                                AuthorName = reader["AuthorName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                ReleaseDate = (int)reader["ReleaseDate"],
                                BookStatus = reader["BookStatus"].ToString()
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            if (books.Count > 0)
            {
                var model = new BookListViewModel
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    Books = books
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("GetAllBooksUI");
            }


            //string connString = _configuration.GetConnectionString("MyDbConnection");

            //string query = @"SELECT b.BookId, b.BookName, b.ReleaseDate, b.BookStatus,
            //         a.AuthorName, c.CategoryName FROM Books b 
            //         INNER JOIN Authors a ON b.AuthorId = a.AuthorId
            //         INNER JOIN Categories c ON b.CategoryId = c.CategoryId";

            //List<BookViewModel> books = new List<BookViewModel>();

            //using (SqlConnection connection = new SqlConnection(connString))
            //{
            //    connection.Open();
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                BookViewModel book = new BookViewModel
            //                {
            //                    BookId = (Guid)reader["BookId"],
            //                    BookName = reader["BookName"].ToString(),
            //                    AuthorName = reader["AuthorName"].ToString(),
            //                    CategoryName = reader["CategoryName"].ToString(),
            //                    ReleaseDate = (int)reader["ReleaseDate"],
            //                    BookStatus = reader["BookStatus"].ToString()
            //                };
            //                books.Add(book);
            //            }
            //        }
            //    }
            //}

            //if (books.Count > 0)
            //{
            //    return View(books);
            //}
            //else
            //{
            //    return NotFound();
            //}

            //var bookList = new List<BookListViewModel>();
            //HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Book/GetAllBooks").Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    string data = response.Content.ReadAsStringAsync().Result;
            //    bookList = JsonConvert.DeserializeObject<List<BookListViewModel>>(data);

            //}

            //return View(bookList);
        }

        [HttpGet]
        public IActionResult CreateBookUI(string test)
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBookUI()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateBookUI(string test)
        {
            return View();
        }

        [HttpPut]
        public IActionResult UpdateBookUI()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult DeleteBookUI()
        {
            return View();
        }

    }
}
