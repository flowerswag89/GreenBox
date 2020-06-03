using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GreenBox.Weather
{
    class WeatherResponce
    {
        public string Name { get; set; }

        public WeatherInfo[] Weather { get; set; }

        public TemperatureInfo Main { get; set; }
    }
}
