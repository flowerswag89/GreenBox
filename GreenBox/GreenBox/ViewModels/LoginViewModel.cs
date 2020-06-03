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
using GreenBox.ViewModels.PagesViewModels;
using GreenBox.Models;
using GreenBox.Views;
using GreenBox.DBClasses;
using GreenBox.Views.Pages;
using GreenBox.Controls;

namespace GreenBox.ViewModels
{
    class LoginViewModel: INotifyPropertyChanged
    {

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private RelayCommand logInCommand;
        public RelayCommand LogInCommand
        {
            get
            {
                return logInCommand ??
                  (logInCommand = new RelayCommand(obj =>
                  {
                      if (string.IsNullOrEmpty(Username) || Username.Length > 30)
                      {
                          CustomMessageBox.Show("Input login!(max length: 30)");
                          return;
                      }
                      else if(string.IsNullOrEmpty(Username) || Password.Length > 30)
                      {
                          CustomMessageBox.Show("Input password!(max length: 30)");
                          return;
                      }
                      else
                      {
                          int user_id = LoginModel.ConfirmLogin(Username, Password);
                          if(user_id != 0)
                          {
                              MainWindow mv = MainWindow.Initialize(user_id, new HomePage());
                              (obj as Window).Close();
                              mv.ShowDialog();
                              return;
                          }
                          else
                          {
                              CustomMessageBox.Show("You enter wrong login or password");
                              return;
                          }
                      }
                  }));
            }
        }

        private RelayCommand referenceCommand1;
        public RelayCommand ReferenceCommand1
        {
            get
            {
                return referenceCommand1 ??
                    (referenceCommand1 = new RelayCommand(obj =>
                    {
                        RegisterWindow rw = new RegisterWindow();
                        (obj as Window).Close();
                        rw.ShowDialog();

                    }));
            }
        }

        private RelayCommand referenceCommand2;
        public RelayCommand ReferenceCommand2
        {
            get
            {
                return referenceCommand2 ??
                    (referenceCommand2 = new RelayCommand(obj =>
                    {
                        ForgotPasswordWindow fpw = new ForgotPasswordWindow();
                        (obj as Window).Close();
                        fpw.ShowDialog();

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
