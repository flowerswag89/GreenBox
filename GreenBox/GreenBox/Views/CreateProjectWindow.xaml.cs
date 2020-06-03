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
using GreenBox.ViewModels;

namespace GreenBox.Views
{
    /// <summary>
    /// Логика взаимодействия для ProjectCreateWindow.xaml
    /// </summary>
    public partial class CreateProjectWindow : Window
    {
        public CreateProjectWindow(int userid)
        {
            InitializeComponent();
            DataContext = new CreateProjectViewModel(userid);
        }
    }
}
