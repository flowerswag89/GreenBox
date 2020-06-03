using GreenBox.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;

namespace GreenBox.Models
{
    class ForgotPasswordModel
    {
        private static int code;
        public static string SendMessage(string username, string message)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    User user = (from u in context.Users
                                 where u.Username == username
                                 select u).FirstOrDefault();

                    if (user != null || username == "me")
                    {
                        string emailAdress;
                        if (username != "me")
                            emailAdress = user.Email;
                        else
                            emailAdress = "greenbox55556@gmail.com";
                        MailAddress fromMailAdress = new MailAddress("greenbox55556@gmail.com", "GreenBox team");
                        MailAddress toMailAdress = new MailAddress(emailAdress);

                        using (MailMessage mailMessage = new MailMessage(fromMailAdress, toMailAdress))
                        using (SmtpClient smtpClient = new SmtpClient())
                        {
                            code = CodeGenerator();

                            mailMessage.Subject = "GreenBox email verificator";

                            if (username != "me")
                                mailMessage.Body = $"Hello, its yout verify code: {code}. You can reestablish your account.";
                            else
                                mailMessage.Body = message;
                            smtpClient.Host = "smtp.gmail.com";
                            smtpClient.Port = 587;
                            smtpClient.EnableSsl = true;
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new NetworkCredential(fromMailAdress.Address, "Greenabc555");

                            smtpClient.Send(mailMessage);

                            return $"Verify code was sended to email:{toMailAdress.Address}";
                        }
                    }
                    else
                    {
                        return "User whith this email not found";
                    }

                }
                catch { return "Error"; }
            }
        }

        public static int CodeGenerator()
        {
            Random rndm = new Random();
            return rndm.Next(1000, 9999);
        }

        public static bool VerifyCode(int user_code)
        {
            if (code == user_code)
                return true;
            return false;
        }

        public static bool PasswordUpdater(string username, string password)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    byte[] arrayForPassword = Encoding.UTF8.GetBytes(password);

                    SHA1 sha = new SHA1CryptoServiceProvider();
                    byte[] hashPassord = sha.ComputeHash(arrayForPassword);

                    User user = (from u in context.Users
                                 where u.Username == username
                                 select u).FirstOrDefault();
                    user.Password = hashPassord;

                    context.SaveChanges();
                    return true;
                }
                catch { return false; }
            }
        }

        public static int GetId(string username)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    User user = (from u in context.Users
                                 where u.Username == username
                                 select u).FirstOrDefault();
                    if (user != null)
                        return user.Id;
                    else
                        return 0;
                }
                catch { return 0; }
            }
        }
    }
}
