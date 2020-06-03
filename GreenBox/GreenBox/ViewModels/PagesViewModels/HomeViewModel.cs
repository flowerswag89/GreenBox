using GreenBox.DBClasses;
using GreenBox.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GreenBox.ViewModels.PagesViewModels
{
    class HomeViewModel:INotifyPropertyChanged
    {

        private string currentCity;
        public string CurrentCity
        {
            get { return currentCity; }
            set
            {
                currentCity = value;
                OnPropertyChanged("CurrentCity");
            }
        }

        private string currentDiscription;
        public string CurrentDiscription
        {
            get { return currentDiscription; }
            set
            {
                currentDiscription = value;
                OnPropertyChanged("CurrentDiscription");
            }
        }

        private byte[] currentWeatherImage;
        public byte[] CurrentWeatherImage
        {
            get { return currentWeatherImage; }
            set
            {
                currentWeatherImage = value;
                OnPropertyChanged("CurrentWeatherImage");
            }
        }

        private string weatherNumber;
        public string WeatherNumber
        {
            get { return weatherNumber; }
            set
            {
                weatherNumber = value;
                OnPropertyChanged("WeatherNumber");
            }
        }

        private string currentDate;
        public string CurrentDate
        {
            get { return currentDate; }
            set
            {
                currentDate = value;
                OnPropertyChanged("CurrentDate");
            }
        }

        private byte[] plantImage;
        public byte[] PlantImage
        {
            get { return plantImage; }
            set
            {
                plantImage = value;
                OnPropertyChanged("PlantImage");
            }
        }

        private string plantName;
        public string PlantName
        {
            get { return plantName; }
            set
            {
                plantName = value;
                OnPropertyChanged("PlantName");
            }
        }


        private string plantType;
        public string PlantType
        {
            get { return plantType; }
            set
            {
                plantType = value;
                OnPropertyChanged("PlantType");
            }
        }

        private string plantCost;
        public string PlantCost
        {
            get { return plantCost; }
            set
            {
                plantCost = value;
                OnPropertyChanged("PlantCost");
            }
        }

        private string plantInfo;
        public string PlantInfo
        {
            get { return plantInfo; }
            set
            {
                plantInfo = value;
                OnPropertyChanged("PlantInfo");
            }
        }


        [Obsolete]
        public HomeViewModel()
        {
            CurrentCity = HomeModel.GetCity();
            CurrentDate = DateTime.Now.DayOfWeek.ToString() + " " + DateTime.Now.Day.ToString() + " " + DateTime.Now.Month.ToString() + " " + DateTime.Now.Year.ToString();
            var tuple = HomeModel.GetWhether(CurrentCity);
            CurrentDiscription = tuple.Item2;
            CurrentWeatherImage = tuple.Item3;
            WeatherNumber = tuple.Item4 + "°C";

            Random rndm = new Random();

            int day;
            if (DateTime.Now.Day > 20)
                day = DateTime.Now.Day - 20;
            else
                day = DateTime.Now.Day;

            Element element = HomeModel.GetElement(day);
            if(element != null)
            {
                PlantImage = element.SideImage;
                PlantName = element.Name;
                PlantType = element.Type;
                PlantCost = element.Cost + "$";
                PlantInfo = element.Info;
            }
        }

        private RelayCommand readMoreCommand;
        public RelayCommand ReadMoreCommand
        {
            get
            {
                return readMoreCommand ??
                  (readMoreCommand = new RelayCommand(obj =>
                  {
                      Process.Start($"https://en.wikipedia.org/wiki/{PlantName}");
                      return;
                  }));
            }
        }

        private RelayCommand showInstaCommand;
        public RelayCommand ShowInstaCommmand
        {
            get
            {
                return showInstaCommand ??
                  (showInstaCommand = new RelayCommand(obj =>
                  {
                      Process.Start($"https://www.instagram.com/zelenka.studio/");
                      return;
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
