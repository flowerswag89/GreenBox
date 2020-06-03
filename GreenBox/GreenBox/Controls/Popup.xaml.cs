using GreenBox.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GreenBox.Controls
{
    /// <summary>
    /// Логика взаимодействия для Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {
        private static Popup p;
        private static DispatcherTimer timer = new DispatcherTimer();
        public Popup()
        {
            InitializeComponent();
        }

        public static void Show(string message, Window mw)
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Close);
            p = new Popup();
            p.Owner = mw;
            p.WindowStartupLocation = WindowStartupLocation.Manual;
            p.Left = mw.Left + mw.Width/2;
            p.Top = mw.Top + mw.Height / 2 - 200;
            p.Message.Text = message;
            timer.Start();
            p.ShowDialog();
        }

        public static void Close(object sender, EventArgs e)
        {
            timer.Stop();
            p.Close();
        }
    }
}
