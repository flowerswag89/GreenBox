using GreenBox.DBClasses;
using GreenBox.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;

namespace GreenBox.Models
{
    class SettingsModel
    {
        public static User GetUser(int user_id)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    User user = (from u in context.Users
                                 where u.Id == user_id
                                 select u).FirstOrDefault();

                    return user;
                }
                catch { return null; }
            }
        }

        public static  bool Verificator(string _string)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    if (_string.Contains("@"))
                    {
                        var users = from u in context.Users
                                    where u.Email == _string
                                    select u;

                        if (users.Count() > 0)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        var users = from u in context.Users
                                    where u.Username == _string
                                    select u;

                        if (users.Count() > 0)
                            return false;
                        else
                            return true;
                    }
                }
                catch { return false; }
            }
        }

        public static bool UserUpdate(User old_user, string newPassord)
        {
            byte[] hashNewPassword;
            byte[] arrayForPassword;
            SHA1 sha = new SHA1CryptoServiceProvider();
            using (DataContext context = new DataContext())
            {
                if (string.IsNullOrEmpty(newPassord))
                {
                    hashNewPassword = old_user.Password;
                }
                else
                {
                    arrayForPassword = Encoding.UTF8.GetBytes(newPassord);
                    hashNewPassword = sha.ComputeHash(arrayForPassword);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        User user = (from u in context.Users
                                     where u.Id == old_user.Id
                                     select u).FirstOrDefault();

                        if (user.Icon != old_user.Icon)
                            user.Icon = old_user.Icon;
                        if (user.Surname != old_user.Surname)
                            user.Surname = old_user.Surname;
                        if (user.Name != old_user.Name)
                            user.Name = old_user.Name;
                        if (user.Email != old_user.Email)
                        {
                            if (Verificator(old_user.Email))
                                user.Email = old_user.Email;
                            else
                            {
                                CustomMessageBox.Show("User with this email is exist");
                                throw new Exception();
                            }
                        }
                        if (user.Username != old_user.Username)
                        {
                            if (Verificator(old_user.Username))
                                user.Username = old_user.Username;
                            else
                            {
                                CustomMessageBox.Show("User with rhis username is exist");
                                throw new Exception();
                            }
                        }
                        if (user.Password != old_user.Password)
                            user.Password = hashNewPassword;
                        else
                        {
                            CustomMessageBox.Show("You enter wrong password");
                            throw new Exception();
                        }

                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }   
            }
        }
    }
}
