using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace GreenBox.Controls
{
    /// <summary>
    /// Логика взаимодействия для ToggleButton.xaml
    /// </summary>
    public partial class ToggleButton : UserControl
    {
        Thickness LeftSide = new Thickness(-39, 0, 0, 0);
        Thickness RightSide = new Thickness(0, 0, -39, 0);
        SolidColorBrush Off = new SolidColorBrush(Color.FromRgb(160, 160, 160));
        SolidColorBrush On = new SolidColorBrush(Color.FromRgb(1, 255, 103));

        public ToggleButton()
        {
            InitializeComponent();
            Back.Fill = Off;
            IsToggled = false;
            Dot.Margin = LeftSide;
        }

        public bool IsToggled
        {
            get { return (bool)GetValue(IsToggledProperty); }
            set { SetValue(IsToggledProperty, value); }
        }

        public static readonly DependencyProperty IsToggledProperty =
            DependencyProperty.Register("IsToggled", typeof(bool),
            typeof(ToggleButton));

        private void Dot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsToggled)
            {
                Back.Fill = On;
                IsToggled = true;
                Dot.Margin = RightSide;

            }
            else
            {

                Back.Fill = Off;
                IsToggled = false;
                Dot.Margin = LeftSide;

            }
        }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsToggled)
            {
                Back.Fill = On;
                IsToggled = true;
                Dot.Margin = RightSide;

            }
            else
            {

                Back.Fill = Off;
                IsToggled = false;
                Dot.Margin = LeftSide;

            }

        }
    }
}
