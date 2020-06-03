using GreenBox.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GreenBox.ViewModels
{
    class VkViewModel : INotifyPropertyChanged
    {
        public class VkJsonTokenResponse
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string user_id { get; set; }
        }

        public string App_id { get; set; }
        public string Secret { get; set; }
        public string Code { get; set; }

        public VkViewModel(byte[] _image)
        {
            MemoryStream ms = new MemoryStream(_image, 0, _image.Length);
            ms.Write(_image, 0, _image.Length);
            Image image = Image.FromStream(ms, true);
            image.Save("..\\..\\Temp\\tmpimg.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }


        public static string Upload(string userid, string token, string imagePath, string text)
        {
            var c = new WebClient();
            //
            var u = "https://api.vk.com/method/photos.getWallUploadServer?user_id=" + userid
                    + "&access_token=" + token + "&v=5.57";
            var r = c.DownloadString(u);
            var j = JsonConvert.DeserializeObject(r) as JObject;
            //
            var u2 = j["response"]["upload_url"].ToString();
            var r2 = Encoding.UTF8.GetString(c.UploadFile(u2, "POST", imagePath));
            var j2 = JsonConvert.DeserializeObject(r2) as JObject;
            //
            var u3 = "https://api.vk.com/method/photos.saveWallPhoto?access_token=" + token
                     + "&server=" + j2["server"]
                     + "&photo=" + j2["photo"]
                     + "&hash=" + j2["hash"] + "&v=5.57";

            var r3 = c.DownloadString(u3);
            var j3 = JsonConvert.DeserializeObject(r3) as JObject;

            // 
            var u4 = "https://api.vk.com/method/wall.post?access_token=" + token
                     + "&owner_id" + j3["response"][0]["owner_id"]
                     + "&message=" + text
                     + $"&attachments=photo{userid}_" + j3["response"][0]["id"] + "&v=5.57";

            File.Delete("..\\..\\Temp\\tmpimg.jpg");

            return c.DownloadString(u4);
        }





    public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
