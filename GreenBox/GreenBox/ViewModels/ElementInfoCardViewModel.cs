using GreenBox.Controls;
using GreenBox.DBClasses;
using GreenBox.Models;
using GreenBox.ViewModels.PagesViewModels;
using GreenBox.Views;
using GreenBox.Views.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GreenBox.ViewModels
{
    class ElementInfoCardViewModel: INotifyPropertyChanged
    {
        private int elementId;
        private int userId;

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

        private string buttonVisibility;
        public string ButtonVisibility
        {
            get { return buttonVisibility; }
            set
            {
                buttonVisibility = value;
                OnPropertyChanged("ButtonVisibility");
            }
        }

        private int buttonWidth;
        public int ButtonWidth
        {
            get { return buttonWidth; }
            set
            {
                buttonWidth = value;
                OnPropertyChanged("ButtonWidth");
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

        public ElementInfoCardViewModel(Element element, int user_id, Window window)
        {

            elementId = element.Id;
            PlantImage = element.SideImage;
            PlantName = element.Name;
            PlantCost = element.Cost + "$";
            PlantInfo = element.Info;

            userId = user_id;
            ButtonVisibility = "Collapsed";
            ButtonWidth = 260;

            if (element.UserId != null)
            {
                ButtonVisibility = "Visible";
                ButtonWidth = 170;
            }
        }



        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj =>
                  {
                      var messageBoxResult = CustomMessageBoxYesNo.Show("Delete user element", "Are you sure?", MessageBoxButton.YesNo);
                      if (messageBoxResult == MessageBoxResult.Yes)
                      {
                          if (ElementInfoCardModel.DeleteUserElement(elementId))
                          {
                              (obj as Window).Close();
                              CustomMessageBox.Show("Element was deleted");
                              MainWindow mw = MainWindow.Initialize(userId, new ProjectPage(userId));
                          }
                          else
                              CustomMessageBox.Show("Error");
                      }
                      return;
                  }));
            }
        }




        private RelayCommand closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand ??
                  (closeCommand = new RelayCommand(obj =>
                  {
                      (obj as Window).Close();

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
