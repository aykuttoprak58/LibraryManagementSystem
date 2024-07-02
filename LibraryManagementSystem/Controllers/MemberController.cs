using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace LibraryManagementSystemUI.Controllers
{
    public class MemberController : Controller
    {
        private readonly IConfiguration _configuration;
        public MemberController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllMembersUI()
        {
            string Name = HttpContext.Session.GetString("Name");
            string Surname = HttpContext.Session.GetString("Surname");
            TempData["username"] = Name + " " + Surname;
            string MemberShipType = HttpContext.Session.GetString("MemberShipType");
            TempData["role"] = MemberShipType;
            string connString = _configuration.GetConnectionString("MyDbConnection");

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string selectQuery = "Select * from members";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    using (var reader = com.ExecuteReader())
                    {
                        var members = new List<Members>();

                        while (reader.Read())
                        {

                            var member = new Members()
                            {
                                MemberId = (Guid)reader["MemberId"],
                                Name = (string)reader["Name"],
                                Surname = (string)reader["Surname"],
                                UserName = (string)reader["UserName"],
                                Email = (string)reader["Email"],
                                BirthDate = ((DateTime)reader["BirthDate"]),
                                IdentityNumber = (string)reader["IdentityNumber"],
                                MemberShipType = (string)reader["MemberShipType"],
                                MemberShipStatusId = (int)reader["MemberShipStatusId"],
                                PasswordResetToken = (string)reader["PasswordResetToken"],
                                ResetTokenExpires = (DateTime)reader["ResetTokenExpires"]
                            };

                            members.Add(member);
                        }

                        if (members != null)
                        {
                            return View(members);
                        }

                        else
                        {

                            return View();  
                        }
                    }
                }
            }
        }

        [HttpDelete]
        public IActionResult DeleteMemberUI()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult GetMemberStatusUI()
        {
            string Name = HttpContext.Session.GetString("Name");
            string Surname = HttpContext.Session.GetString("Surname");
            TempData["username"] = Name + " " + Surname;
            string MemberShipType = HttpContext.Session.GetString("MemberShipType");
            TempData["role"] = MemberShipType;
            //string MemberShipType = HttpContext.Session.GetString("MemberShipType");
            //string MemberId = HttpContext.Session.GetString("MemberId");

            //if (MemberShipType == "Admin")
            //{
            string connString = _configuration.GetConnectionString("MyDbConnection");

                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    string selectQuery = "Select * from MemberStatus";
                    using (SqlCommand com = new SqlCommand(selectQuery, con))
                    {
                        using (var reader = com.ExecuteReader())
                        {
                            var members = new List<MemberStatus>();

                            while (reader.Read())
                            {
                                var member = new MemberStatus()
                                {
                                    MemberId = (Guid)reader["MemberId"],
                                    MemberFullName = (string)reader["MemberFullName"],
                                    IdentityNumber = (string)reader["IdentityNumber"],
                                    BarrowedBookNumber = (int)reader["BarrowedBookNumber"],
                                    ReturnedBookNumber = (int)reader["ReturnedBookNumber"],
                                    PunishmentNumber = (int)reader["PunishmentNumber"],
                                    PunishmentStatus = (string)reader["PunishmentStatus"]
                                };

                                members.Add(member);
                            }

                            if (members != null)
                            {
                                return View(members);
                            }
                        }

                    }

                }
            //}

            return View();
        }

    }
}
