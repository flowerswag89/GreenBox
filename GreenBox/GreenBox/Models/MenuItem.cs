using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox.Models
{
    class MenuItem
    {
        public byte[] Img { get; set; }
        public string Type { get; set; }

        public MenuItem(byte[] img, string type)
        {
            Img = img;
            Type = type;
        }

        public static ObservableCollection<MenuItem> GetMenuItemCollection()
        {
            ObservableCollection<MenuItem> oc = new ObservableCollection<MenuItem>();

            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-tree.png")), "Tree"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-bush.png")), "Bush"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-flower.png")), "Flower"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-apple.png")), "FruitVegetables"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-fountain.png")), "FountainLake"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-house.png")), "House"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-road.png")), "Road"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-cover.png")), "Cover"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-chair.png")), "Furniture"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-stone.png")), "Design"));
            oc.Add(new MenuItem(ImageToByteArray(Image.FromFile("..\\..\\Resources\\Icons\\MenuIcons\\solid-user.png")), "User"));

            return oc;

        }

        public static byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
