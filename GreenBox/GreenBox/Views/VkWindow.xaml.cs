using GreenBox.Controls;
using GreenBox.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GreenBox.Views
{
    /// <summary>
    /// Логика взаимодействия для VkWindow.xaml
    /// </summary>
    public partial class VkWindow : Window
    {
        public VkWindow(byte[] _image)
        {
            InitializeComponent();

            VkViewModel vkvm = new VkViewModel(_image);
            string App_id = "7458880";
            WBrowser.Source = new Uri($"https://oauth.vk.com/authorize?client_id={App_id}&scope=8196&redirect_uri=http://api.vkontakte.ru/blank.html&display=popup&response_type=token&v=5.57", UriKind.Absolute);
        }

        private void WebBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

            try
            {
                string url = e.Uri.ToString();
                string l = url.Split('#')[1];

                if (l[0] == 'a')
                {
                    string token = l.Split('&')[0].Split('=')[1];
                    string id = l.Split('=')[3];

                    string path = "..\\..\\Temp\\tmpimg.jpg";

                    string s = VkViewModel.Upload(id, token, path, "See, what I can dooo");

                    if (!s.Contains("eroor"))
                    {
                        CustomMessageBox.Show("Image was pasted");
                    }
                    else
                    {
                        CustomMessageBox.Show("Error!");
                    }

                    this.Close();
                }

            }
            catch { }
        }
    }
 
}
