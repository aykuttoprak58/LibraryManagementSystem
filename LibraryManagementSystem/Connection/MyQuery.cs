using LibraryManagementSystem.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Hangfire;
using LibraryManagementSystemApi.Models;
using System.Data.SqlClient;

namespace LibraryManagementSystem.Connetcion
{
    public static class MyQuery
    {
        private const string ConnectionString = "Data Source=.;Initial Catalog=LIBRARY;Integrated Security=True;TrustServerCertificate=True";

        public static string GetConnectionString()
        {
            return ConnectionString;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        public static Members GetUserByUserName(string UserName)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "Select * From Members Where UserName = @UserName";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@UserName", UserName);
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Members member = new Members
                            {
                                Name = (string)reader["Name"],
                                Surname = (string)reader["Surname"],
                                UserName = (string)reader["UserName"],
                                IdentityNumber = (string)reader["IdentityNumber"],
                                MemberShipType = (string)reader["MemberShipType"],
                                MemberShipStatusId = (int)reader["MemberShipStatusId"],
                                PasswordHash = (byte[])reader["PasswordHash"],
                                PasswordSalt = (byte[])reader["PasswordSalt"],
                                MemberId = (Guid)reader["MemberId"]

                            };
                            return member;
                        }

                        return null;
                    }
                }
            }
        }

        public static Members GetUserByUserEmail(string email)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "Select * From Members Where Email = @Email";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@Email", email);
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Members member = new Members
                            {
                                Email = (string)reader["Email"],
                                Name = (string)reader["Name"],
                                Surname = (string)reader["Surname"],
                                UserName = (string)reader["UserName"]
                            };
                            return member;
                        }

                        return null;
                    }
                }
            }
        }

        public static Members GetUserByResetToken(string username, string passwordResetToken)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "Select * From Members Where PasswordResetToken = @PasswordResetToken and UserName = @UserName";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@PasswordResetToken", passwordResetToken);
                    com.Parameters.AddWithValue("@UserName", username);

                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Members user = new Members
                            {
                                UserName = (string)reader["UserName"],
                                PasswordResetToken = (string)reader["PasswordResetToken"],
                                ResetTokenExpires = (DateTime)reader["ResetTokenExpires"]
                            };
                            return user;
                        }

                        return null;
                    }
                }
            }
        }

        public static string UpdateBookStatus(string bookId, string bookStatus)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = GetConnectionString();
                con.Open();
                string updateQuery = "UPDATE Books Set BookStatus = @BookStatus Where BookId = @BookId";
                using (SqlCommand com = new SqlCommand())
                {
                    com.CommandText = updateQuery;
                    com.Connection = con;
                    com.Parameters.AddWithValue("@BookId", bookId);
                    com.Parameters.AddWithValue("@BookStatus", bookStatus);
                    var affected = com.ExecuteNonQuery();
                    if (affected > 0)
                    {
                        return "Book has been updated";
                    }
                    else
                    {
                        return "Failed to update book";
                    }
                }
            }
        }


        public static string DeleteMemberStatus(Guid MemberId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.ConnectionString = GetConnectionString();
                con.Open();
                string deleteQuery = "delete from MemberStatus where MemberId = @MemberId";
                using (SqlCommand com = new SqlCommand(deleteQuery, con))
                {
                    com.Parameters.AddWithValue("@MemberId", MemberId);
                    var affected = com.ExecuteNonQuery();

                    if (affected > 0)
                    {
                        return "MemberStatus has been deleted";
                    }
                    else
                    {
                        return "Failed to delete MemberStatus";
                    }
                }

            }

        }





        public static string BookStatusCheck(Guid bookId)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = GetConnectionString();
                con.Open();
                string selectQuery = "SELECT * FROM Books WHERE BookId = @BookId";
                using (SqlCommand com = new SqlCommand())
                {
                    com.CommandText = selectQuery;
                    com.Connection = con;
                    com.Parameters.AddWithValue("@BookId", bookId);

                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var book = new Books()
                            {
                                BookStatus = (string)reader["BookStatus"]
                            };

                            if (book != null)
                            {
                                return book.BookStatus.ToString();
                            }
                        }

                        return "book is not found";
                    }
                }
            }
        }


        public static string CreateMemberStatus(MemberStatus memberStatus)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string insertQuery = "INSERT INTO MemberStatus (MemberId,MemberFullName,IdentityNumber,BarrowedBookNumber,ReturnedBookNumber,PunishmentNumber,PunishmentStatus,PunishmentMailStatus) VALUES (@MemberId,@MemberFullName,@IdentityNumber,@BarrowedBookNumber,@ReturnedBookNumber,@PunishmentNumber,@PunishmentStatus,@PunishmentMailStatus)";
                using (SqlCommand com = new SqlCommand(insertQuery, con))
                {
                    com.Parameters.AddWithValue("@MemberId", memberStatus.MemberId);
                    com.Parameters.AddWithValue("@MemberFullName", memberStatus.MemberFullName);
                    com.Parameters.AddWithValue("@IdentityNumber", memberStatus.IdentityNumber);
                    com.Parameters.AddWithValue("@BarrowedBookNumber", 0);
                    com.Parameters.AddWithValue("@ReturnedBookNumber", 0);
                    com.Parameters.AddWithValue("@PunishmentNumber", 0);
                    com.Parameters.AddWithValue("@PunishmentStatus", memberStatus.PunishmentStatus);
                    com.Parameters.AddWithValue("@PunishmentMailStatus", memberStatus.PunishmentMailStatus);
                    var rowAffected = com.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        return "New Member Status Created";
                    }
                    else
                    {
                        return "Failed to ceate Member Status";
                    }
                }
            }
        }

        public static int BBNCheck(string identityNumber)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = GetConnectionString();
                con.Open();
                string selectQuery = "SELECT * FROM MemberStatus WHERE IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand())
                {
                    com.CommandText = selectQuery;
                    com.Connection = con;
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);

                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var memberStatus = new MemberStatus()
                            {
                                BarrowedBookNumber = (int)reader["BarrowedBookNumber"]
                            };

                            if (memberStatus != null)
                            {
                                return memberStatus.BarrowedBookNumber;
                            }
                        }

                        return 0;
                    }
                }
            }
        }

        public static int RBNCheck(string identityNumber)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = GetConnectionString();
                con.Open();
                string selectQuery = "SELECT * FROM MemberStatus WHERE IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand())
                {
                    com.CommandText = selectQuery;
                    com.Connection = con;
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);

                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var memberStatus = new MemberStatus()
                            {
                                ReturnedBookNumber = (int)reader["ReturnedBookNumber"]
                            };

                            if (memberStatus != null)
                            {
                                return memberStatus.ReturnedBookNumber;
                            }

                            return memberStatus.ReturnedBookNumber;
                        }

                        return 0;
                    }
                }
            }
        }

        public static int PunishmentNumberCheck(string identityNumber)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                //con.ConnectionString = GetConnectionString();
                con.Open();
                string selectQuery = "SELECT * FROM MemberStatus WHERE IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    //com.CommandText = selectQuery;
                    //com.Connection = con;
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);

                    using (var reader = com.ExecuteReader())
                    {
                        var memberstatuss = new List<MemberStatus>();
                        while (reader.Read())
                        {

                            var memberStatus = new MemberStatus()
                            {
                                PunishmentNumber = (int)reader["PunishmentNumber"]
                            };

                            if (memberStatus != null)
                            {
                                return memberStatus.PunishmentNumber;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        return 0;
                    }
                }
            }
        }

        public static string UpdatePunishmentStatus(string punishmentStatus, string identityNumber)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = GetConnectionString();
                con.Open();
                string selectQuery = "UPDATE MemberStatus SET PunishmentStatus = @PunishmentStatus  WHERE IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand())
                {
                    com.CommandText = selectQuery;
                    com.Connection = con;
                    com.Parameters.AddWithValue("@PunishmentStatus", punishmentStatus);
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    var rowAffected = com.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        return "işlem başarıllı";
                    }

                    else
                    {
                        return "işlem başarısız";
                    }
                }
            }
        }

        public static string PunishmentStatusCheck(string identityNumber)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "select * from MemberStatus where  IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var memberStatus = new MemberStatus()
                            {
                                IdentityNumber = (string)reader["IdentityNumber"],
                                PunishmentStatus = (string)reader["PunishmentStatus"],
                                PunishmentMailStatus = (bool)reader["PunishmentMailStatus"]
                            };

                            if (memberStatus.PunishmentStatus == "2 Defa Ceza Aldı" && memberStatus.PunishmentMailStatus == true)
                            {
                                return "2 Defa Ceza Aldı";
                            }

                            else if (memberStatus.PunishmentStatus == "4 Defa Ceza Aldı" && memberStatus.PunishmentMailStatus == false)
                            {
                                return "4 Defa Ceza Aldı";
                            }
                            else
                            {
                                return "Ceza Yok";
                            }
                        }

                        return "Ceza Yok";
                    }
                }
            }
        }

        public static void UpdatePunishmentMailStatus(string identityNumber, bool mailStatus)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "Update MemberStatus set PunishmentMailStatus = @PunishmentMailStatus WHERE IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@PunishmentMailStatus", mailStatus);
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    com.ExecuteNonQuery();
                }
            }
        }





        public static string UpdatePunishmentNumberPlus(string IdentityNumber)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string updateQuery = "Update MemberStatus Set PunishmentNumber = @PunishmentNumber Where IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand(updateQuery, con))
                {

                    var jj = PunishmentNumberCheck(IdentityNumber);
                    com.Parameters.AddWithValue("@IdentityNumber", IdentityNumber);
                    com.Parameters.AddWithValue("@PunishmentNumber", jj + 1);
                    var rowAffected = com.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        return "BBN has been updated";
                    }

                    else
                    {
                        return "Failed to update";
                    }
                }
            }
        }



        public static string UpdateBBNPlus(string identityNumber)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string updateQuery = "Update MemberStatus Set BarrowedBookNumber = @BarrowedBookNumber Where IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand(updateQuery, con))
                {

                    var BBNnumber = BBNCheck(identityNumber);
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    com.Parameters.AddWithValue("@BarrowedBookNumber", BBNnumber + 1);
                    var rowAffected = com.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        return "BBN has been updated";
                    }

                    else
                    {
                        return "Failed to update";
                    }
                }
            }
        }


        public static string UpdateBBNMinus(string identityNumber)
        {
            var BBNumber = BBNCheck(identityNumber);
            var RBNumber = RBNCheck(identityNumber);
            var BBNumber2 = BBNumber - 1;
            var RBNumber2 = RBNumber + 1;

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string updateQuery = "UPDATE MemberStatus SET BarrowedBookNumber = @BarrowedBookNumber, ReturnedBookNumber = @ReturnedBookNumber WHERE IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand(updateQuery, con))
                {


                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    com.Parameters.AddWithValue("@BarrowedBookNumber", BBNumber2);
                    com.Parameters.AddWithValue("@ReturnedBookNumber", RBNumber2);
                    var rowAffected = com.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {


                        return "BBN has been updated";
                    }

                    else
                    {
                        return "Failed to update";
                    }
                }
            }
        }


        public static DateTime CheckBarrowBookDate(Guid barrowedBookId)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = GetConnectionString();
                con.Open();
                string selectQuery = "SELECT * FROM BorrowedBooks WHERE BarrowedBookId = @BarrowedBookId";
                using (SqlCommand com = new SqlCommand())
                {
                    com.CommandText = selectQuery;
                    com.Connection = con;
                    com.Parameters.AddWithValue("@BarrowedBookId", barrowedBookId);

                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var bookDate = new BorrowedBooks()
                            {
                                ReturnDate = (DateTime)reader["ReturnDate"]
                            };

                            if (bookDate != null)
                            {
                                return bookDate.ReturnDate;
                            }
                        }
                        return Convert.ToDateTime("1993-01-01 00:00:00");
                    }
                }
            }
        }

        public static string identityNumberCheck(string identityNumber, Guid returnedBookId)
        {

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "select * from BorrowedBooks where IdentityNumber = @IdentityNumber and BarrowedBookId = @BarrowedBookId";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    com.Parameters.AddWithValue("@BarrowedBookId", returnedBookId);
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var books = new BorrowedBooks()
                            {
                                IdentityNumber = (string)reader["IdentityNumber"]
                            };

                            if (books != null)
                            {
                                return books.IdentityNumber;
                            }

                            else
                            {
                                return null;
                            }
                        }
                        return "0";
                    }
                }
            }

        }


        public static string deleteBarrowedBook(Guid barrowedBookId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "delete from BorrowedBooks where BarrowedBookId = @BarrowedBookId";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@BarrowedBookId", barrowedBookId);
                    var rowAffected = com.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        return "Barrowed book has been deleted";
                    }

                    else
                    {

                        return "An error occurred";
                    }
                }
            }
        }


        public static string UpdateMemberShipStatusId(string identityNumber, int StatusId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "Update Members Set MemberShipStatusId = @MemberShipStatusId where IdentityNumber = @IdentityNumber";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    com.Parameters.AddWithValue("@MemberShipStatusId", StatusId);
                    var rowAffected = com.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        if (StatusId == 2)
                        {
                            return "The member has been deactivated.";
                        }
                        else if (StatusId == 3)
                        {
                            return "The member has been canceled.";
                        }

                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static string PasswordResetToken(string PasswordResetToken, DateTime ResetTokenExpires, string memberEmail)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string updateUser = "UPDATE Members SET PasswordResetToken = @PasswordResetToken, ResetTokenExpires = @ResetTokenExpires WHERE Email = @Email";

                using (SqlCommand com = new SqlCommand(updateUser, con))
                {
                    com.Parameters.AddWithValue("@PasswordResetToken", PasswordResetToken);
                    com.Parameters.AddWithValue("@ResetTokenExpires", ResetTokenExpires);
                    com.Parameters.AddWithValue("@Email", memberEmail);

                    var rowsAffected = com.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return "New User was updated";
                    }

                    else
                    {
                        return "Failed to update new user";
                    }

                }
            }
        }


        public static string ResetPassword(string password, string passwordResetToken)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                CreatePasswordHash(password, out byte[] newPasswordHash, out byte[] newPasswordSalt);
                var resetTokenExpires = Convert.ToDateTime("2000-01-01 00:00:00.000");
                string updateUser = "UPDATE Members SET PasswordHash = @PasswordHash,PasswordSalt = @PasswordSalt , ResetTokenExpires=@ResetTokenExpires WHERE PasswordResetToken = @PasswordResetToken";

                using (SqlCommand com = new SqlCommand(updateUser, con))
                {
                    com.Parameters.AddWithValue("@PasswordHash", newPasswordHash);
                    com.Parameters.AddWithValue("@PasswordSalt", newPasswordSalt);
                    com.Parameters.AddWithValue("@PasswordResetToken", passwordResetToken);
                    com.Parameters.AddWithValue("@ResetTokenExpires", resetTokenExpires);

                    var rowsAffected = com.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return "Member was updated";
                    }

                    else
                    {
                        return "Failed to update user";
                    }

                }
            }
        }

        public static string SetPasswordResetTokenNull(string passwordResetToken)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                var resetTokenExpires = Convert.ToDateTime("2000-01-01 00:00:00.000");
                string updateUser = "UPDATE Members SET PasswordResetToken = '' WHERE PasswordResetToken = @PasswordResetToken";

                using (SqlCommand com = new SqlCommand(updateUser, con))
                {
                    com.Parameters.AddWithValue("@PasswordResetToken", passwordResetToken);
                    var rowsAffected = com.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return "PasswordResetToken was updated";
                    }

                    else
                    {
                        return "Failed to update PasswordResetToken";
                    }

                }
            }
        }


        public static async Task<string> SendEmail(Email email)
        {
            try
            {
                string fromMail = "softwarechannel58@gmail.com";
                string fromPassword = "comv qems wcff keaj";
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromMail);
                mailMessage.Subject = "Şifre Yenileme";
                mailMessage.To.Add(new MailAddress("softwarechannel58@gmail.com"));
                mailMessage.Body =
                $"Merhaba Sayın, {email.FullName}\n" +
                $"Parola Sıfırlama Tokenı : {email.ResetToken}\n";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true,
                };

                await smtpClient.SendMailAsync(mailMessage);

                return "Mail was send successfully";
            }
            catch (Exception ex)
            {
                return "Failed to send email " + ex.ToString();
            }
        }

        public static async Task<string> SendEmail2(BorrowedBooks book)
        {
            if (MessageControl(1) == true)
            {
                try
                {
                    string fromMail = "softwarechannel58@gmail.com";
                    string fromPassword = "comv qems wcff keaj";
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(fromMail);
                    mailMessage.Subject = "Kitap İade Hatırlatma";
                    mailMessage.To.Add(new MailAddress("softwarechannel58@gmail.com"));
                    mailMessage.Body =
                    $"Merhaba Sayın, {book.MemberFullName}\n" +
                    $"{book.BarrowedBookName.ToUpper()} adlı kitabı iade etmeniz için 7 gün kalmıştır.\n" +
                    $"İade Tarihi : {book.ReturnDate.ToShortDateString()}\n";
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromMail, fromPassword),
                        EnableSsl = true,
                    };

                    await smtpClient.SendMailAsync(mailMessage);

                    return "Mail was send successfully";
                }
                catch (Exception ex)
                {
                    return "Failed to send email " + ex.ToString();
                }
            }

            else
            {
                return "Message Setting is not activated";
            }
        }

        public static async Task<string> SendEmail3(BorrowedBooks book)
        {
            if (MessageControl(2) == true)
            {
                try
                {
                    string fromMail = "softwarechannel58@gmail.com";
                    string fromPassword = "comv qems wcff keaj";
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(fromMail);
                    mailMessage.Subject = "Kitap İade Hatırlatma";
                    mailMessage.To.Add(new MailAddress("softwarechannel58@gmail.com"));
                    mailMessage.Body =
                    $"Merhaba Sayın, {book.MemberFullName}\n" +
                    $"{book.BarrowedBookName.ToUpper()} adlı kitabı iade etmeniz için bugün son gündür.\n" +
                    $"Lütfen iade ediniz. İade tarihini geçerseniz üyeliğiniz pasif hale getirilip 100 TL cezai işlem uygulanacaktır.\n" +
                    $"İade Tarihi: {book.ReturnDate.ToShortDateString()}\n";
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromMail, fromPassword),
                        EnableSsl = true,
                    };

                    await smtpClient.SendMailAsync(mailMessage);

                    return "Mail was send successfully";
                }
                catch (Exception ex)
                {
                    return "Failed to send email " + ex.ToString();
                }
            }

            else
            {
                return "Message Setting is not activated";
            }
        }

        public static async Task<string> SendEmail4(BorrowedBooks book)
        {
            if (MessageControl(3) == true)
            {
                try
                {
                    string fromMail = "softwarechannel58@gmail.com";
                    string fromPassword = "comv qems wcff keaj";
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(fromMail);
                    mailMessage.Subject = "Cezai İşlem";
                    mailMessage.To.Add(new MailAddress("softwarechannel58@gmail.com"));
                    mailMessage.Body =
                    $"Merhaba Sayın, {book.MemberFullName}\n" +
                    $"{book.BarrowedBookName.ToUpper()} adlı kitabı iade etmediğiniz için ceza yediniz.\n" +
                    $"İade Tarihi: {book.ReturnDate.ToShortDateString()}\n" +
                    $"Bugün: {DateTime.Now.ToShortDateString()}\n";
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromMail, fromPassword),
                        EnableSsl = true,
                    };

                    await smtpClient.SendMailAsync(mailMessage);
                    return "Mail was send successfully";
                }
                catch (Exception ex)
                {
                    return "Failed to send email " + ex.ToString();
                }
            }

            else
            {
                return "Message Setting is not activated";
            }


        }

        public static async Task<string> SendEmail5(BorrowedBooks book)
        {

            if (MessageControl(4) == true)
            {
                try
                {
                    string fromMail = "softwarechannel58@gmail.com";
                    string fromPassword = "comv qems wcff keaj";
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(fromMail);
                    mailMessage.Subject = "Cezai İşlem";
                    mailMessage.To.Add(new MailAddress("softwarechannel58@gmail.com"));
                    mailMessage.Body =
                    $"Merhaba Sayın, {book.MemberFullName}\n" +
                    $"2 kez üst üste ceza yediğiniz için üyeliğiniz pasif hale getirilmiştir.30 gün sonra tekrar aktif hale getirilecektir.\n" +
                    $"İade Tarihi: {book.ReturnDate.ToShortDateString()}\n" +
                    $"Bugün: {DateTime.Now.ToShortDateString()}\n";
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromMail, fromPassword),
                        EnableSsl = true,
                    };

                    await smtpClient.SendMailAsync(mailMessage);

                    return "Mail was send successfully";
                }
                catch (Exception ex)
                {
                    return "Failed to send email " + ex.ToString();
                }
            }

            else
            {
                return "Message Setting is not activated";
            }



        }

        public static async Task<string> SendEmail6(BorrowedBooks book)
        {
            if (MessageControl(5) == true)
            {
                try
                {
                    string fromMail = "softwarechannel58@gmail.com";
                    string fromPassword = "comv qems wcff keaj";
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(fromMail);
                    mailMessage.Subject = "Cezai İşlem";
                    mailMessage.To.Add(new MailAddress("softwarechannel58@gmail.com"));
                    mailMessage.Body =
                    $"Merhaba Sayın, {book.MemberFullName}\n" +
                    $"4 kez üst üste ceza yediğiniz için üyeliğiniz tamamen iptal edilmiştir.\n" +
                    $"İade Tarihi: {book.ReturnDate.ToShortDateString()}\n" +
                    $"Bugün: {DateTime.Now.ToShortDateString()}\n";
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromMail, fromPassword),
                        EnableSsl = true,
                    };

                    await smtpClient.SendMailAsync(mailMessage);

                    return "Mail was send successfully";
                }
                catch (Exception ex)
                {
                    return "Failed to send email " + ex.ToString();
                }
            }

            else
            {
                return "Message Setting is not activated";
            }

        }

        public static async Task<string> SendEmail7(BorrowedBooks book, DateTime activeDate)
        {
            try
            {
                string fromMail = "softwarechannel58@gmail.com";
                string fromPassword = "comv qems wcff keaj";
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromMail);
                mailMessage.Subject = "Cezai İşlem";
                mailMessage.To.Add(new MailAddress("softwarechannel58@gmail.com"));
                mailMessage.Body =
                $"Merhaba Sayın, {book.MemberFullName}\n" +
                $"Üyeliğiniz tekrar aktif hale getirilmiştir.\n" +
                $"Pasif Hale Getirildiği Tarih: {DateTime.Now.ToShortDateString()}\n" +
                $"Aktif Hale Getirildiği Tarih: {activeDate.ToShortDateString()}\n";
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true,
                };

                await smtpClient.SendMailAsync(mailMessage);

                return "Mail was send successfully";
            }
            catch (Exception ex)
            {
                return "Failed to send email " + ex.ToString();
            }
        }

        public static async Task<List<BorrowedBooks>> BookReturnDateCheck()
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string dateQuery = "Select * from BorrowedBooks";
                using (SqlCommand com = new SqlCommand(dateQuery, con))
                {
                    using (var reader = com.ExecuteReader())
                    {
                        var books = new List<BorrowedBooks>();

                        while (reader.Read())
                        {
                            var book = new BorrowedBooks()
                            {
                                MemberFullName = (string)reader["MemberFullName"],
                                BarrowedBookName = (string)reader["BarrowedBookName"],
                                ReturnDate = (DateTime)reader["ReturnDate"],
                                IdentityNumber = (string)reader["IdentityNumber"]
                            };
                             books.Add(book);
                        }

                        if (books != null)
                        {

                            foreach (var book in books)
                            {
                                var dayTime = book.ReturnDate.Date - DateTime.Now.Date;
                                var dayTime2 = DateTime.Now.Date - book.ReturnDate.Date;

                                if (dayTime.TotalDays == 7)
                                {
                                   await SendEmail2(book);
                                }

                                if (dayTime.TotalDays == 0)
                                {
                                    await SendEmail3(book);
                                }

                                if (dayTime2.TotalDays == 1)
                                {
                                    UpdatePunishmentNumberPlus(book.IdentityNumber);

                                    await SendEmail4(book);
                                }


                                if (PunishmentNumberCheck(book.IdentityNumber) == 2)
                                {
                                    UpdatePunishmentStatus("2 Defa Ceza Aldı", book.IdentityNumber);
                                    UpdateMemberShipStatusId(book.IdentityNumber, 2);

                                    var currentDate = DateTime.Now;
                                    var date1 = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 07, 00, 00);
                                    var date2 = date1.AddDays(30);
                                    //BackgroundJob.Schedule(() => Console.WriteLine("BackgrounJob has been started"), date2);

                                    if (CheckPunishmentMailStatus(book.IdentityNumber) == true)
                                    {
                                        await SendEmail5(book);
                                        BackgroundJob.Schedule(() => UpdateMemberShipStatusId(book.IdentityNumber, 1), date2);
                                        BackgroundJob.Schedule(() => SendEmail7(book, date2), date2);
                                    }


                                    //if (PunishmentStatusCheck(book.IdentityNumber) == "2 Defa Ceza Aldı" && PunishmentNumberCheck(book.IdentityNumber) == 2)
                                    //{
                                    //    SendEmail5(book);
                                    //    BackgroundJob.Schedule(() => UpdateMemberShipStatusId(book.IdentityNumber, 1), date2);
                                    //    BackgroundJob.Schedule(() => SendEmail7(book, date2), date2);
                                    //}

                                    UpdatePunishmentMailStatus(book.IdentityNumber, false);
                                }
                                var IndetityNumberCheck = Convert.ToInt32(PunishmentNumberCheck(book.IdentityNumber));

                                if (IndetityNumberCheck == 4)
                                {
                                    UpdatePunishmentStatus("4 Defa Ceza Aldı", book.IdentityNumber);
                                    UpdateMemberShipStatusId(book.IdentityNumber, 3);

                                    if (CheckPunishmentMailStatus(book.IdentityNumber) == false)
                                    {
                                        await SendEmail6(book);
                                    }

                                    //if (PunishmentStatusCheck(book.IdentityNumber) == "4 Defa Ceza Aldı" && PunishmentNumberCheck(book.IdentityNumber) == 4)
                                    //{

                                    //SendEmail6(book);

                                    //}

                                    UpdatePunishmentMailStatus(book.IdentityNumber, true);
                                }

                            }
                            return  books;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static List<BorrowedBooks> GetAllBorrowedBooks(string MemberShipType, string IdentityNumber)
        {
            if (MemberShipType == "Admin")
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    con.Open();
                    string selectQuery = "SELECT * FROM BorrowedBooks";
                    using (SqlCommand com = new SqlCommand(selectQuery, con))
                    {
                        using (var reader = com.ExecuteReader())
                        {
                            var books = new List<BorrowedBooks>();

                            while (reader.Read())
                            {
                                var book = new BorrowedBooks()
                                {
                                    BarrowedBookId = (Guid)reader["BarrowedBookId"],
                                    BarrowedBookName = (string)reader["BarrowedBookName"],
                                    MemberFullName = (string)reader["MemberFullName"],
                                    IdentityNumber = (string)reader["IdentityNumber"],
                                    BorrowedDate = (DateTime)reader["BorrowedDate"],
                                    ReturnDate = (DateTime)reader["ReturnDate"]
                                };

                                books.Add(book);
                            }

                            if (books != null)
                            {
                                return books;
                            }

                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    con.Open();
                    string selectQuery = "SELECT * FROM BorrowedBooks where IdentityNumber = @IdentityNumber";
                    using (SqlCommand com = new SqlCommand(selectQuery, con))
                    {
                        com.Parameters.AddWithValue("@IdentityNumber", IdentityNumber);
                        using (var reader = com.ExecuteReader())
                        {
                            var books = new List<BorrowedBooks>();

                            while (reader.Read())
                            {
                                var book = new BorrowedBooks()
                                {
                                    BarrowedBookId = (Guid)reader["BarrowedBookId"],
                                    BarrowedBookName = (string)reader["BarrowedBookName"],
                                    MemberFullName = (string)reader["MemberFullName"],
                                    IdentityNumber = (string)reader["IdentityNumber"],
                                    BorrowedDate = (DateTime)reader["BorrowedDate"],
                                    ReturnDate = (DateTime)reader["ReturnDate"]
                                };

                                books.Add(book);
                            }

                            if (books != null)
                            {
                                return books;
                            }

                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
        }

        public static List<ReturnedBooks> GetAllReturnedBooks(string MemberShipType, string IdentityNumber)
        {
            if (MemberShipType == "Admin")
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    con.Open();
                    string selectQuery = "select * from ReturnedBooks";
                    using (SqlCommand com = new SqlCommand(selectQuery, con))
                    {
                        using (var reader = com.ExecuteReader())
                        {
                            var books = new List<ReturnedBooks>();

                            while (reader.Read())
                            {
                                var book = new ReturnedBooks()
                                {
                                    ReturnedBookId = (Guid)reader["ReturnedBookId"],
                                    ReturnedBookName = (string)reader["ReturnedBookName"],
                                    MemberFullName = (string)reader["MemberFullName"],
                                    IdentityNumber = (string)reader["IdentityNumber"],
                                    Deadline = (DateTime)reader["Deadline"],
                                    ReturnDate = (DateTime)reader["ReturnDate"],
                                    PunishmentStatus = (string)reader["PunishmentStatus"]
                                };
                                books.Add(book);
                            }
                            if (books != null)
                            {
                                return books;
                            }

                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    con.Open();
                    string selectQuery = "select * from ReturnedBooks where IdentityNumber = @IdentityNumber";
                    using (SqlCommand com = new SqlCommand(selectQuery, con))
                    {
                        com.Parameters.AddWithValue("@IdentityNumber", IdentityNumber);
                        using (var reader = com.ExecuteReader())
                        {
                            var books = new List<ReturnedBooks>();

                            while (reader.Read())
                            {
                                var book = new ReturnedBooks()
                                {
                                    ReturnedBookId = (Guid)reader["ReturnedBookId"],
                                    ReturnedBookName = (string)reader["ReturnedBookName"],
                                    MemberFullName = (string)reader["MemberFullName"],
                                    IdentityNumber = IdentityNumber,
                                    Deadline = (DateTime)reader["Deadline"],
                                    ReturnDate = (DateTime)reader["ReturnDate"],
                                    PunishmentStatus = (string)reader["PunishmentStatus"]
                                };
                                books.Add(book);
                            }
                            if (books != null)
                            {
                                return books;
                            }

                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
        }

        public static string GetBookNameByBookId(Guid bookId)
        {

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "select * from books where BookId = @BookId";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@BookId", bookId);
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Books book = new Books()
                            {
                                BookId = (Guid)reader["BookId"],
                                BookName = (string)reader["BookName"],
                                AuthorId = (int)reader["AuthorId"],
                                CategoryId = (int)reader["CategoryId"],
                                ReleaseDate = (int)reader["ReleaseDate"],
                                BookStatus = (string)reader["BookStatus"],
                            };
                            if (book != null || book.BookName != "")
                            {

                                return book.BookName;
                            }
                        }

                        return "There is no book with this number";
                    }
                }
            }
        }

        public static bool MessageControl(int messageId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "Select * from Settings where SettingId = @SettingId";
                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@SettingId", messageId);
                    com.ExecuteNonQuery();
                    using (var reader = com.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            var setting = new Settings()
                            {
                                SettingId = (int)reader["SettingId"],
                                SettingName = (string)reader["SettingName"],
                                SettingStatus = (bool)reader["SettingStatus"]
                            };

                            if (setting.SettingStatus == true)
                            {
                                return true;
                            }

                            else
                            {
                                return false;
                            }
                        }

                    }
                }
            }
            return false;
        }

        public static void UpdateMessageSetting(bool settingStatus, int settingId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string updateQuery = "Update Settings Set SettingStatus = @SettingStatus Where SettingId = @SettingId";
                using (SqlCommand com = new SqlCommand(updateQuery, con))
                {
                    com.Parameters.AddWithValue("@SettingId", settingId);
                    com.Parameters.AddWithValue("@SettingStatus", settingStatus);
                    com.ExecuteNonQuery();
                }
            }
        }


        public static void UpdateAllMessageSetting(bool settingStatus)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string updateQuery = "Update Settings Set SettingStatus = @SettingStatus";
                using (SqlCommand com = new SqlCommand(updateQuery, con))
                {
                    com.Parameters.AddWithValue("@SettingStatus", settingStatus);
                    com.ExecuteNonQuery();
                }
            }
        }


        public static bool CheckPunishmentMailStatus(string identityNumber)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string selectQuery = "Select PunishmentMailStatus from MemberStatus where IdentityNumber = @IdentityNumber";

                using (SqlCommand com = new SqlCommand(selectQuery, con))
                {
                    com.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var mailStatus = new MemberStatus()
                            {
                                PunishmentMailStatus = (bool)reader["PunishmentMailStatus"]
                            };

                            if (mailStatus.PunishmentMailStatus == true)
                            {
                                return true;
                            }

                            else
                            {
                                return false;
                            }
                        }

                        return false;
                    }
                }
            }
        }


    }
}
