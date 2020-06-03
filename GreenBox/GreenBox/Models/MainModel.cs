using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenBox.DBClasses;
using GreenBox;

namespace GreenBox.Models
{
    class MainModel
    {

        public static User GetUser(int user_id)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    User user = (from u in context.Users
                                 where u.Id == user_id
                                 select u).First();
                    return user;
                }
                catch { return null; }
            }
        }

    }
}
