using GreenBox.DBClasses;
using GreenBox.Views.Pages;
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

namespace GreenBox.Views
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow single = null;

        public static  MainWindow Initialize(int user_id, Page currentPage)
        {
            if (single == null)
                single = new MainWindow(user_id);
            else
                (single.DataContext as ViewModels.MainViewModel).CurrentPageSetter(currentPage);
            return single;
        }

        protected MainWindow(int user_id)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.MainViewModel(user_id, new HomePage());
        }
    }
}
