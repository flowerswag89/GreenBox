using GreenBox.Models;
using GreenBox.ViewModels;
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
    /// Логика взаимодействия для ConstructorPage.xaml
    /// </summary>
    public partial class ConstructorPage : Page
    {
        public ConstructorPage(int project_id, int user_id)
        {
            InitializeComponent();
            DataContext = new ConstructorViewModel(project_id, user_id, this);
        }
    }
}
