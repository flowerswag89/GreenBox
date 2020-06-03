using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Drawing;
using System.Windows;
using GreenBox.Weather;
using RestSharp;
using GreenBox.Models.City;
using System.Collections;
using GreenBox.DBClasses;

namespace GreenBox.Models
{
    class HomeModel
    {
        [Obsolete]
        public static string GetCity()
        {
            try
            {
                string myHost = Dns.GetHostName();
                string myIP = Dns.GetHostByName(myHost).AddressList[0].ToString();



                string strIpLocation = string.Empty;

                string url = "https://ipapi.co/json/";

                var client = new RestClient(url);
                var request = new RestRequest()
                {
                    Method = Method.GET
                };
                var responce = client.Execute(request);
                strIpLocation = responce.Content;

                CityResponce cr = JsonConvert.DeserializeObject<CityResponce>(strIpLocation);
                return cr.City;
            }
            catch { return ""; }
        }

        public static (string, string, byte[], int) GetWhether(string city)
        {
            try
            {
                string url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=metric&appid=2346f27428e58418899df7cbaf76c849";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string response;

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                WeatherResponce ow = JsonConvert.DeserializeObject<WeatherResponce>(response);

                var tuple = (ow.Name, ow.Weather[0].description, ow.Weather[0].Icon, (int)ow.Main.temp);

                return tuple;
            }
            catch { return ("","",null,0); }
        }

        public static Element GetElement(int element_id)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    Element element = (from e in context.Elements
                                       where e.Id == element_id
                                       select e).FirstOrDefault();
                    return element;
                }
                catch { return null; }
            }
        }
    }
}
