using LibraryManagementSystem.Connetcion;
using LibraryManagementSystem.Dto;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LibraryManagementSystem.Models;
using LibraryManagementSystemApi.Dto;
using System.Reflection;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace LibraryManagementSystemUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignInUI()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignInUI(SignInDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = MyQuery.GetUserByUserName(model.UserName);

            if (user == null)
            {
                ViewData["Message3"] = "Kullanıcı Adı yada Şifreyi Hatalı Tekrar Deneyiniz";
                return View();
            }
            if (!MyQuery.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                ViewData["Message3"] = "Şifreyi Hatalı Girdiniz Tekrar Deneyiniz";
                return View();
            }

            if (user.MemberShipStatusId == 2)
            {
                ViewData["Message3"] = "Hesabınız devre dışı bırakıldı, lütfen ceza süresi dolduktan sonra tekrar deneyin.";
                return View();
            }

            if (user.MemberShipStatusId == 3)
            {
                ViewData["Message3"] = "Hesabınız iptal edildi, lütfen destek ekibiyle iletişime geçin";
                return View();
            }

            HttpContext.Session.SetString("Username", model.UserName);
            HttpContext.Session.SetString("Name", user.Name);
            HttpContext.Session.SetString("Surname", user.Surname);
            HttpContext.Session.SetString("MemberShipType", user.MemberShipType);
            HttpContext.Session.SetString("IdentityNumber", user.IdentityNumber);
            HttpContext.Session.SetString("MemberId", user.MemberId.ToString());

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name + " " + user.Surname),
            // Kullanıcı rolleri buraya eklenmeli
            new Claim(ClaimTypes.Role, user.MemberShipType) // Örnek rol
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {

            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            string token = CreateToken(user);

            Response.Cookies.Append("jwt", token, new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true
            });

            return RedirectToAction("GetAllBooksUI", "Book");

        }

        private string CreateToken(Members member)
        {
            var roles = MyQuery.GetUserByUserName(member.UserName);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,roles.UserName),
                new Claim(ClaimTypes.Role,member.MemberShipType),

            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [HttpGet]
        public IActionResult SignOutUI()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Name");
            HttpContext.Session.Remove("Surname");
            HttpContext.Session.Remove("IdentityNumber");
            HttpContext.Session.Remove("MemberId");
            HttpContext.Session.Clear();
            return RedirectToAction("SignInUI");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUpUI()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUpUI(UserDto request)
        {
       
            if (!ModelState.IsValid)
            {
                // If model state is invalid, return the view with validation errors
                return View(request);
            }
            var userNameCheck = MyQuery.GetUserByUserName(request.UserName);
            var userEmailCheck = MyQuery.GetUserByUserEmail(request.Email);
            if (userNameCheck != null)
            {
                ViewData["Message1"] = "Kullanıcı Adı Sistemde Kayıtlıdır";
                return View(request);
            }

           
            if (userEmailCheck != null)
            {
                ViewData["Message2"] = "E-mail sistemde Kayıtlıdır";
                return View(request);
            }
            MyQuery.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Guid guid = Guid.NewGuid();
            var resetTokenExpires = Convert.ToDateTime("2000-01-01 00:00:00.000");
            var passwordResetToken = "";
            var memberShipStatusId = 1;
            var gender = "";
            var memberShipType = "";

            if (request.MemberShipType == "Student")
            {
                memberShipType = "Student";

            }

            else
            {
                memberShipType = "Citizen";
            }
            

            if (request.Gender == "MALE")
            {
                gender = "MALE";

            }

            else
            {
                gender = "FEMALE";
            }
            string memberName = request.Name.Substring(0, 1).ToUpper() + request.Name.Substring(1).ToLower();
            string memberSurname = request.Surname.Substring(0, 1).ToUpper() + request.Surname.Substring(1).ToLower();

            var memberStatus = new MemberStatus();
            memberStatus.MemberFullName = memberName + " " + memberSurname;
            memberStatus.IdentityNumber = request.IdentityNumber;
            memberStatus.MemberId = guid;
            memberStatus.PunishmentStatus = "Ceza Yok";
            memberStatus.PunishmentNumber = 0;
            memberStatus.PunishmentMailStatus = true;

            MyQuery.CreateMemberStatus(memberStatus);

            string connString = _configuration.GetConnectionString("MyDbConnection");

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string insertQuery = "INSERT INTO Members (MemberId,Name,Surname,BirthDate,IdentityNumber,Gender,MemberShipType,MemberShipStatusId,UserName,Email,PasswordHash,PasswordSalt,PasswordResetToken,ResetTokenExpires)" +
                    "VALUES (@MemberId,@Name,@Surname,@BirthDate,@IdentityNumber,@Gender,@MemberShipType,@MemberShipStatusId,@UserName,@Email,@PasswordHash,@PasswordSalt,@PasswordResetToken,@ResetTokenExpires)";
                using (SqlCommand com = new SqlCommand(insertQuery, con))
                {
  
                    request.MemberId = guid;

                    com.Parameters.AddWithValue("@MemberId", request.MemberId);
                    com.Parameters.AddWithValue("@Name", memberName);
                    com.Parameters.AddWithValue("@Surname", memberSurname);
                    com.Parameters.AddWithValue("@BirthDate", request.BirthDate);
                    com.Parameters.AddWithValue("@IdentityNumber", request.IdentityNumber);
                    com.Parameters.AddWithValue("@Gender", gender);
                    com.Parameters.AddWithValue("@MemberShipType", memberShipType);
                    com.Parameters.AddWithValue("@MemberShipStatusId", memberShipStatusId);
                    com.Parameters.AddWithValue("@UserName", request.UserName);
                    com.Parameters.AddWithValue("@Email", request.Email);
                    com.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    com.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
                    com.Parameters.AddWithValue("@PasswordResetToken", passwordResetToken);
                    com.Parameters.AddWithValue("@ResetTokenExpires", resetTokenExpires);
                    var affectedRow = com.ExecuteNonQuery();

                    if (affectedRow > 0)
                    {
                        return RedirectToAction("SignInUI");
                    }

                    else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordUI()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordUI(EmailDto emailDto)
        {
   
            if (!ModelState.IsValid)
            {
                return View();
            }
            var member = MyQuery.GetUserByUserEmail(emailDto.Email);

            if (member != null)
            {
                TempData["UserName"] = member.UserName;
                Random random = new Random();
                string PasswordResetToken = random.Next(100000, 999999).ToString();
                var ResetTokenExpires = DateTime.Now.AddDays(1);
                MyQuery.PasswordResetToken(PasswordResetToken, ResetTokenExpires, emailDto.Email);
                Email request = new Email();
                request.FullName = member.Name + " " + member.Surname;
                request.ResetToken = PasswordResetToken;
                await MyQuery.SendEmail(request);

                return RedirectToAction("ResetPasswordUI");
            }

            else
            {
                ViewData["Message"] = "E-posta Sistemde kayıtlı değildir";
                return View(emailDto);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordUI()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPasswordUI(PasswordDto request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            string userName = TempData["UserName"].ToString();

            var userCheck = MyQuery.GetUserByUserName(userName);
            if (userCheck == null)
            {
                return View();

            }
            var userCheck2 = MyQuery.GetUserByResetToken(userName, request.PasswordResetToken);
            if (userCheck2 == null || userCheck2.ResetTokenExpires < DateTime.Now || userCheck2.PasswordResetToken == "")
            {
                return View();
            }

            MyQuery.ResetPassword(request.Password, request.PasswordResetToken);
            MyQuery.SetPasswordResetTokenNull(request.PasswordResetToken);
            return RedirectToAction("SignInUI");
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult SettingsUI()
        {
            bool messageEnabled1 = MyQuery.MessageControl(1);
            ViewData["MessageEnabled1"] = messageEnabled1;
            bool messageEnabled2 = MyQuery.MessageControl(2);
            ViewData["MessageEnabled2"] = messageEnabled2;
            bool messageEnabled3 = MyQuery.MessageControl(3);
            ViewData["MessageEnabled3"] = messageEnabled3;
            bool messageEnabled4 = MyQuery.MessageControl(4);
            ViewData["MessageEnabled4"] = messageEnabled4;
            bool messageEnabled5 = MyQuery.MessageControl(5);
            ViewData["MessageEnabled5"] = messageEnabled5;
            bool messageEnabled6 = MyQuery.MessageControl(6);
            ViewData["MessageEnabled6"] = messageEnabled6;
            bool messageEnabled7 = MyQuery.MessageControl(7);
            ViewData["MessageEnabled7"] = messageEnabled7;

            string Name = HttpContext.Session.GetString("Name");
            string Surname = HttpContext.Session.GetString("Surname");
            TempData["username"] = Name + " " + Surname;
            string MemberShipType = HttpContext.Session.GetString("MemberShipType");
            TempData["role"] = MemberShipType;
            return View();
        }


        [HttpPost]
        public IActionResult ToggleMessage1(bool enabled)
        {
            MyQuery.UpdateMessageSetting(enabled, 1);
            return RedirectToAction("SettingsUI");
        }

        [HttpPost]
        public IActionResult ToggleMessage2(bool enabled)
        {
            MyQuery.UpdateMessageSetting(enabled, 2);
            return RedirectToAction("SettingsUI");
        }

        [HttpPost]
        public IActionResult ToggleMessage3(bool enabled)
        {
            MyQuery.UpdateMessageSetting(enabled, 3);
            return RedirectToAction("SettingsUI");
        }

        [HttpPost]
        public IActionResult ToggleMessage4(bool enabled)
        {
            MyQuery.UpdateMessageSetting(enabled, 4);
            return RedirectToAction("SettingsUI");
        }

        [HttpPost]
        public IActionResult ToggleMessage5(bool enabled)
        {
            MyQuery.UpdateMessageSetting(enabled, 5);
            return RedirectToAction("SettingsUI");
        }

        [HttpPost]
        public IActionResult ToggleMessage6(bool enabled)
        {
            MyQuery.UpdateMessageSetting(enabled, 6);
            return RedirectToAction("SettingsUI");
        }

        [HttpPost]
        public IActionResult ToggleMessage7(bool enabled)
        {
            MyQuery.UpdateAllMessageSetting(enabled);
            return RedirectToAction("SettingsUI");
        }
    }
}
