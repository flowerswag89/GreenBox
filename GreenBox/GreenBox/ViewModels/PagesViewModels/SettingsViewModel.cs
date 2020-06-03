using GreenBox.Controls;
using GreenBox.DBClasses;
using GreenBox.Models;
using GreenBox.Views;
using GreenBox.Views.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;

namespace GreenBox.ViewModels.PagesViewModels
{
    class SettingsViewModel: INotifyPropertyChanged
    {
        private User user;

        private byte[] userIcon;
        public byte[] UserIcon
        {
            get { return userIcon; }
            set
            {
                userIcon = value;
                OnPropertyChanged("UserIcon");
            }
        }

        private string surnameProperty;
        public string SurnameProperty
        {
            get { return surnameProperty; }
            set
            {
                surnameProperty = value;
                OnPropertyChanged("SurnameProperty");
            }
        }

        private string nameProperty;
        public string NameProperty
        {
            get { return nameProperty; }
            set
            {
                nameProperty = value;
                OnPropertyChanged("NameProperty");
            }
        }

        private string emailProperty;
        public string EmailProperty
        {
            get { return emailProperty; }
            set
            {
                emailProperty = value;
                OnPropertyChanged("EmailProperty");
            }
        }

        private string useernameProperty;
        public string UsernameProperty
        {
            get { return useernameProperty; }
            set
            {
                useernameProperty = value;
                OnPropertyChanged("UsernameProperty");
            }
        }

        private string oldPassword;
        public string OldPassword
        {
            get { return oldPassword; }
            set
            {
                oldPassword = value;
                OnPropertyChanged("OldPassword");
            }
        }

        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }


        public SettingsViewModel(int user_id)
        {
            user = SettingsModel.GetUser(user_id);
            if (user.Icon != null)
                UserIcon = user.Icon;
            else
                UserIcon = AddUserElementModel.GetBytes("..\\..\\Resources\\DefaultImage\\userBig.png");
            SurnameProperty = user.Surname;
            NameProperty = user.Name;
            EmailProperty = user.Email;
            UsernameProperty = user.Username;
            //if(user.Theme != null)
        }

        private bool Validator()
        {
            string pattern = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
            string result_string = "";
            bool result = true;

            if ((string.IsNullOrEmpty(SurnameProperty) || SurnameProperty.Length > 30))
            {
                result_string += "Check your Surname (max length: 30)\n";
                result = false;
            }
            if ((string.IsNullOrEmpty(NameProperty) || NameProperty.Length > 30))
            {
                result_string += "Check your Name (max length: 30)\n";
                result = false;
            }
            if ((string.IsNullOrEmpty(EmailProperty) || !Regex.IsMatch(EmailProperty, pattern)))
            {
                result_string += "Check your Email\n";
                result = false;
            }
            if ((String.IsNullOrEmpty(UsernameProperty) || UsernameProperty.Length > 30))
            {
                result_string += "Check your Username (max length: 30)\n";
                result = false;
            }
            if (!string.IsNullOrEmpty(OldPassword) && (string.IsNullOrEmpty(newPassword) || NewPassword.Length > 30))
            {
                result_string += "Check your new password (max length:30)\n";
                result = false;
            }

            if (result == true)
            {
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

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj =>
                    {
                        var messageBoxResult = CustomMessageBoxYesNo.Show("Save changes", "Are you sure?", MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            byte[] hashOldPassword;
                            if (OldPassword != null)
                            {
                                byte[] arrayForPassword = Encoding.UTF8.GetBytes(OldPassword);

                                SHA1 sha = new SHA1CryptoServiceProvider();
                                hashOldPassword = sha.ComputeHash(arrayForPassword);
                            }
                            else
                                hashOldPassword = user.Password;

                            if (Validator())
                            {
                                if (SettingsModel.UserUpdate(new User()
                                {
                                    Id = user.Id,
                                    Surname = SurnameProperty,
                                    Name = NameProperty,
                                    Email = EmailProperty,
                                    Username = UsernameProperty,
                                    Password = hashOldPassword,
                                    Icon = UserIcon
                                }, newPassword))
                                {
                                    CustomMessageBox.Show("Сhanges were successful");
                                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                                    Application.Current.Shutdown();
                                }
                                else
                                    CustomMessageBox.Show("An error occurred during the changes, please try again");
                            }
                            else
                                return;
                        }


                    }));
            }
        }

        private RelayCommand addImageCommand;
        public RelayCommand AddImageCommand
        {
            get
            {
                return addImageCommand ??
                  (addImageCommand = new RelayCommand(obj =>
                  {

                      try
                      {
                          OpenFileDialog dialog = new OpenFileDialog();
                          dialog.Filter = "txt files (*.jpg)|*.jpg|All files (*.*)|*.*";
                          dialog.InitialDirectory = @"C:\Users\Sivchik Denis\source\repos\WpfApp1\";
                          dialog.Title = "Please select an image file to encrypt.";

                          Nullable<bool> result = dialog.ShowDialog();

                          // Process save file dialog box resultss
                          if (result == true)
                          {
                              UserIcon = AddUserElementModel.GetBytes(dialog.FileName);
                          }
                      }
                      catch { return; }

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
