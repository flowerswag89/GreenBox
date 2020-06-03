using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using GreenBox.ViewModels.PagesViewModels;
using GreenBox.Models;
using GreenBox.DBClasses;
using GreenBox.Views;
using GreenBox.Views.Pages;
using GreenBox.Controls;

namespace GreenBox.ViewModels
{
    class RegisterViewModel : INotifyPropertyChanged
    {
        private string surname;
        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

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

        private string password1;
        public string Password1
        {
            get { return password1; }
            set
            {
                password1 = value;
                OnPropertyChanged("Password1");
            }
        }

        private string password2;
        public string Password2
        {
            get { return password2; }
            set
            {
                password2 = value;
                OnPropertyChanged("Password2");
            }
        }

        private bool FormValidator()
        {
            string pattern = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
            string result_string = "";
            bool result = true;

            if ((string.IsNullOrEmpty(Surname) || Surname.Length > 30))
            {
                result_string += "Check your Surname (max length: 30)\n";
                result = false;
            }
            if ((string.IsNullOrEmpty(Name) || Name.Length > 30))
            {
                result_string += "Check your Name (max length: 30)\n";
                result = false;
            }
            if ((string.IsNullOrEmpty(Email) || !Regex.IsMatch(Email, pattern)))
            {
                result_string += "Check your Email\n";
                result = false;
            }
            if ((string.IsNullOrEmpty(Username) || Username.Length > 30))
            {
                result_string += "Check your Username (max length: 30)\n";
                result = false;
            }
            if ((string.IsNullOrEmpty(Password1) || Password1.Length > 30))
            {
                result_string += "Check your Password(1) (max length: 30)\n";
                result = false;
            }
            if (!(Password1 == Password2))
            {
                result_string += "Your passwords do not match\n";
                result = false;
            }

            if(result == true)
            {
                result = RegisterModel.EmailVerificator(Email);
                if (result == false)
                    result_string += "User whith this email is exists";
                result = RegisterModel.UsernameVerificator(Username);
                if (result == false)
                    result_string += "User whith this username is exists";
            }


            if (result)
                return true;
            else
            {
                CustomMessageBox.Show(result_string);
                return false;
            }
        }

        private RelayCommand signInCommand;
        public RelayCommand SignInCommand
        {
            get
            {
                return signInCommand ??
                  (signInCommand = new RelayCommand(obj =>
                  {
                      if (FormValidator())
                      {
                          int user_id = RegisterModel.UserInsert(Surname, Name, Email, Username, Password1);
                          if (user_id != 0)
                          {
                              MainWindow mw = MainWindow.Initialize(user_id, new HomePage());
                              (obj as Window).Close();
                              mw.ShowDialog();
                          }
                          else
                          {
                              CustomMessageBox.Show("Error adding to database. Please, try again.");
                              return;
                          }
                      }
                      else
                          return;
                      return;
                  }));
            }
        }

        private RelayCommand referenceCommand;
        public RelayCommand ReferenceCommand
        {
            get
            {
                return referenceCommand ??
                  (referenceCommand = new RelayCommand(obj =>
                  {
                      LoginWindow lg = new LoginWindow();
                      (obj as Window).Close();
                      lg.ShowDialog();
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
