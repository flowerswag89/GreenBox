using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox.Weather
{
    class WeatherInfo
    {
        public int id { get; set; }

        public string main { get; set; }

        public string description { get; set; }

        public string icon;

        public byte[] Icon
        {
            get
            {
                System.Drawing.Image imageIn = System.Drawing.Image.FromFile($"..\\..\\Resources\\Icons\\WeatherIcons\\{icon}.png");
                using (var ms = new MemoryStream())
                {
                    imageIn.Save(ms, imageIn.RawFormat);
                    return ms.ToArray();
                }
            }
        }
    }
}
