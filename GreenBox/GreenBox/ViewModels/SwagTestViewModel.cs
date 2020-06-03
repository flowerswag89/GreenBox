using GreenBox.Controls;
using GreenBox.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GreenBox.ViewModels.PagesViewModels
{
    class SwagTestViewModel : INotifyPropertyChanged
    {
        private int maxValue = 0;
        private Window win;

        private double gougeOpacity;
        public double GougeOpacity
        {
            get { return gougeOpacity; }
            set
            {
                gougeOpacity = value;
                OnPropertyChanged("GougeOpacity");
            }
        }

        private string _mediaSource;
        public string MediaSource
        {
            get
            {
                return _mediaSource;
            }
            set
            {
                _mediaSource = value;
                OnPropertyChanged("MediaSource");
            }
        }

        int _angle;
        public int Angle
        {
            get
            {
                return _angle;
            }

            private set
            {
                _angle = value;
                OnPropertyChanged("Angle");
            }
        }

        int _value;
        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (value >= 0 && value <= maxValue/2)
                {
                    _value = value*2;
                    Angle = Convert.ToInt32(value*3.4 - 85);
                    OnPropertyChanged("Value");
                }
            }
        }

        private int value = 0;

        DispatcherTimer timer = new DispatcherTimer();

        public SwagTestViewModel(int project_id, Window w)
        {
            GougeOpacity = 1;
            maxValue = SwagTestModel.GetSwag(project_id);
            win = w;
            Angle = -85;
            timer.Interval = TimeSpan.FromMilliseconds(60);
            timer.Tick += new EventHandler(ValueChanger);
            timer.Start();
        }

        private void ValueChanger(object sender, EventArgs e)
        {
            if (value < maxValue/2)
                Value = ++value;
            else
            {
                timer.Stop();
                GougeOpacity = 0.2;

                timer.Tick -= new EventHandler(ValueChanger);
                timer.Tick += new EventHandler(WindowCloser);

                if (maxValue < 40)
                {
                    MediaSource = "..\\..\\Resources\\Video\\bad.mp4";
                    timer.Interval = TimeSpan.FromSeconds(11);
                    timer.Start();
                }
                else if (maxValue >= 40 && maxValue < 80)
                {
                    MediaSource = "..\\..\\Resources\\Video\\no bad.mp4";
                    timer.Interval = TimeSpan.FromSeconds(13);
                    timer.Start();
                }
                else
                {
                    MediaSource = "..\\..\\Resources\\Video\\congratulations.mp4";
                    timer.Interval = TimeSpan.FromSeconds(13);
                    timer.Start();
                }
            }
        }

        private void WindowCloser(object sender, EventArgs e)
        {
            timer.Stop();
            win.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
