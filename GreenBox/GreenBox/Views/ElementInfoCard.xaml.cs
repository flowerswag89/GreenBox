using GreenBox.DBClasses;
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
using System.Windows.Shapes;

namespace GreenBox.Views
{
    /// <summary>
    /// Логика взаимодействия для ItemInfoCard.xaml
    /// </summary>
    public partial class ElementInfoCard : Window
    {
        public ElementInfoCard(Element element, int user_id)
        {
            InitializeComponent();
            DataContext = new ElementInfoCardViewModel(element, user_id, this);
        }
    }
}
