using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GreenBox.DBClasses;

namespace GreenBox.Models
{
    class RegisterModel
    {
        public static bool EmailVerificator(string email)
        {
            using (var context = new DataContext())
            {
                try
                {
                    User user = context.Users.Where(u => u.Email == email).Select(u => u).FirstOrDefault();

                    if (user != null)
                        return false;
                }
                catch { return false; }
            }
            return true;
        }

        public static bool UsernameVerificator(string username)
        {
            using (var context = new DataContext())
            {
                try
                {
                    User user = context.Users.Where(u => u.Username == username).Select(u => u).FirstOrDefault();

                    if (user != null)
                        return false;
                }
                catch { return false; }
            }
            return true;
        }

        public static int UserInsert(string surname, string name, string email, string username, string password)
        {
            byte[] arrayForPassword = Encoding.UTF8.GetBytes(password);

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashPassord = sha.ComputeHash(arrayForPassword);

            using (var context = new DataContext())
            {
                try
                {
                    User newuser = new User
                    {
                        Surname = surname,
                        Name = name,
                        Email = email,
                        Username = username,
                        Password = hashPassord
                    };
                    context.Users.Add(newuser);
                    context.SaveChanges();
                    return newuser.Id;
                }
                catch { return 0; }
            }
        }
    }
}
