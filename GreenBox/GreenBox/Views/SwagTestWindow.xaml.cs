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
using System.Windows.Shapes;

namespace GreenBox.Views
{
    /// <summary>
    /// Логика взаимодействия для SwagTestWindow.xaml
    /// </summary>
    public partial class SwagTestWindow : Window
    {
        public SwagTestWindow(int project_id)
        {
            InitializeComponent();
            DataContext = new SwagTestViewModel(project_id, this);
        }
    }
}
