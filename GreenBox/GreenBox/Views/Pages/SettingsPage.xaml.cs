using GreenBox.ViewModels.PagesViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GreenBox.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        int userId;

        public SettingsPage(int user_id)
        {
            userId = user_id;
            InitializeComponent();
            DataContext = new SettingsViewModel(user_id);
        }

        private void Bu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = MainWindow.Initialize(userId, this);
            if (Bu.IsToggled == true)
            {
                mw.WindowState = WindowState.Maximized;
            }
            else
            {
                mw.WindowState = WindowState.Normal;
            }


        }
    }
}
