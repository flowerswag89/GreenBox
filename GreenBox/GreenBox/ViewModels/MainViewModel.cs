using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GreenBox.Views.Pages;
using GreenBox.ViewModels.PagesViewModels;
using GreenBox.DBClasses;
using GreenBox.Models;
using System.IO;
using System.Diagnostics;
using GreenBox.Views;
using GreenBox.Controls;

namespace GreenBox.ViewModels
{
    class MainViewModel: INotifyPropertyChanged
    {

        private User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        private byte[] currentImage;
        public byte[] СurrentImage
        {
            get { return currentImage; }
            set
            {
                currentImage = value;
                OnPropertyChanged("СurrentImage");
            }
        }

        private Page currentPage;

        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }


        private Page HomePage { get; set; }
        private Page ProjectPage { get; set; }
        private Page ConstrPage { get; set; }
        private Page SettingsPage { get; set; }

        public MainViewModel(int user_id, Page current_page)
        {
            CurrentUser = MainModel.GetUser(user_id);
            ProjectPage = new ProjectPage(CurrentUser.Id);
            CurrentPageSetter(current_page);
            if (CurrentUser.Icon == null)
            {
                System.Drawing.Image imageIn = System.Drawing.Image.FromFile("..\\..\\Resources\\DefaultImage\\user.png");
                using (var ms = new MemoryStream())
                {
                    imageIn.Save(ms, imageIn.RawFormat);
                    СurrentImage = ms.ToArray();
                }
            }
            else
                СurrentImage = CurrentUser.Icon;

            FullName = CurrentUser.Surname + " " + CurrentUser.Name;
        }

        
        public void CurrentPageSetter(Page currentPage)
        {
            CurrentPage = currentPage;
        }

        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        private Image userImage;

        public Image UserImage
        {
            get { return userImage; }
            set
            {
                userImage = value;
                OnPropertyChanged("UserImage");
            }
        }

        private RelayCommand hBtn_Click;
        public RelayCommand HBtn_Click
        {
            get
            {
                return hBtn_Click ??
                  (hBtn_Click = new RelayCommand(obj =>
                  {
                      CurrentPageSetter(new HomePage());
                  }));
            }
        }

        private RelayCommand pBtn_Click;
        public RelayCommand PBtn_Click
        {
            get
            {
                return pBtn_Click ??
                  (pBtn_Click = new RelayCommand(obj =>
                  {
                      CurrentPageSetter(new ProjectPage(CurrentUser.Id));
                  }));
            }
        }

        private RelayCommand sBtn_Click;
        public RelayCommand SBtn_Click
        {
            get
            {
                return sBtn_Click ??
                  (sBtn_Click = new RelayCommand(obj =>
                  {
                      CurrentPageSetter(new SettingsPage(CurrentUser.Id));
                  }));
            }
        }

        private RelayCommand eBtn_Click;
        public RelayCommand EBtn_Click
        {
            get
            {
                return eBtn_Click ??
                  (eBtn_Click = new RelayCommand(obj =>
                  {
                  var messageBoxResult = CustomMessageBoxYesNo.Show("Exit", "Are you sure?", MessageBoxButton.YesNo);
                  if (messageBoxResult == MessageBoxResult.Yes)
                  {
                          Process.Start("cmd.exe", "/C RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 255");
                          (obj as Window).Close();
                      }
                  }));
            }
        }

        private RelayCommand logOutCommand;
        public RelayCommand LogOutCommand
        {
            get
            {
                return logOutCommand ??
                  (logOutCommand = new RelayCommand(obj =>
                  {
                      System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                      Application.Current.Shutdown();
                  }));
            }
        }

        private RelayCommand getHelpCommand;
        public RelayCommand GetHelpCommand
        {
            get
            {
                return getHelpCommand ??
                  (getHelpCommand = new RelayCommand(obj =>
                  {
                      GetHelpWindow ghp = new GetHelpWindow();
                      ghp.ShowDialog();
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
