using GreenBox.DBClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox.Models
{
    class AddUserElementModel
    {
        public static bool NameImageVerificator(string name, byte[] top_image, byte[] side_image)
        {
            using (var context = new DataContext())
            {
                try
                {
                    Element element1 = context.Elements.Where(u => u.Name == name).Select(u => u).FirstOrDefault();

                    if (element1 != null)
                        return false;
                }
                catch { return false; }
            }
            return true;
        }

        public static byte[] GetBytes(string source)
        {
            Image imageIn = Image.FromFile(source);
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public static bool ElementInsert(byte[] top_image, byte[] side_image,  string name, string info, double cost, int size, int user_id)
        {
            using (var context = new DataContext())
            {
                try
                {
                    Element newelement = new Element
                    {
                        Name = name,
                        Info = info,
                        Cost = cost,
                        TopImage = top_image,
                        SideImage = side_image,
                        Size = size,
                        Type = "User",
                        UserId = user_id
                    };
                    context.Elements.Add(newelement);
                    context.SaveChanges();
                    return true;
                }
                catch { return false; }
            }
        }
    }
}
