using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GreenBox.DBClasses;

namespace GreenBox.Models
{
    class LoginModel
    {
        public static int ConfirmLogin(string username, string password)
        {
            byte[] arrayForPassword = Encoding.UTF8.GetBytes(password);

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashPassord = sha.ComputeHash(arrayForPassword);

            using (var context = new DataContext())
            {
                try
                {
                    User user = (from u in context.Users
                                 where u.Username == username && u.Password == hashPassord
                                 select u).FirstOrDefault();

                    return user.Id;
                }
                catch { return 0; }
            }
        }

    }
}
