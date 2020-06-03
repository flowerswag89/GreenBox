using GreenBox.Controls;
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

namespace GreenBox.ViewModels
{
    class ForgotPasswordViewModel: INotifyPropertyChanged
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

        private int verifyCode;
        public int VerifyCode
        {
            get { return verifyCode; }
            set
            {
                verifyCode = value;
                OnPropertyChanged("VerifyCode");
            }
        }

        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        private string verivyPanelVisibility;
        public string VerifyPanelVisibility
        {
            get { return verivyPanelVisibility; }
            set
            {
                verivyPanelVisibility = value;
                OnPropertyChanged("VerifyPanelVisibility");
            }
        }

        private string passwordPanelVisibility;
        public string PasswordPanelVisibility
        {
            get { return passwordPanelVisibility; }
            set
            {
                passwordPanelVisibility = value;
                OnPropertyChanged("PasswordPanelVisibility");
            }
        }

        private string newPassword1;
        public string NewPassword1
        {
            get { return newPassword1; }
            set
            {
                newPassword1 = value;
                OnPropertyChanged("NewPassword1");
            }
        }

        private string newPassword2;
        public string NewPassword2
        {
            get { return newPassword2; }
            set
            {
                newPassword2 = value;
                OnPropertyChanged("NewPassword2");
            }
        }

        public ForgotPasswordViewModel()
        {
            VerifyPanelVisibility = "Collapsed";
            PasswordPanelVisibility = "Collapsed";
        }


        private RelayCommand sendMessageCommand;
        public RelayCommand SendMessageCommand
        {
            get
            {
                return sendMessageCommand ??
                  (sendMessageCommand = new RelayCommand(obj =>
                  {
                      MessageText = ForgotPasswordModel.SendMessage(Username,"");
                      if (MessageText.Contains("Verify"))
                      {
                          VerifyPanelVisibility = "Visible";
                      }
                  }));
            }
        }

        private RelayCommand verifyCodeCommand;
        public RelayCommand VerifyCodeCommand
        {
            get
            {
                return verifyCodeCommand ??
                  (verifyCodeCommand = new RelayCommand(obj =>
                  {
                      if (ForgotPasswordModel.VerifyCode(VerifyCode))
                      {
                          MessageText = "Code was verified";
                          PasswordPanelVisibility = "Visible";
                      }
                      else
                      {
                          MessageText = "Try anymore";
                      }
                  }));
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
                      if (Validator())
                      {
                          if (ForgotPasswordModel.PasswordUpdater(Username, newPassword1))
                          {
                              int user_id = ForgotPasswordModel.GetId(Username);
                              if (user_id != 0)
                              {
                                  MainWindow mv = MainWindow.Initialize(user_id, new HomePage());
                                  (obj as Window).Close();
                                  mv.ShowDialog();
                                  return;
                              }
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

        private bool Validator()
        {
            string result_string = "";
            bool result = true;

            if ((string.IsNullOrEmpty(NewPassword1) || NewPassword1.Length > 30))
            {
                result_string += "Check your Password(1) (max length: 30)\n";
                result = false;
            }
            if (!(NewPassword1 == NewPassword2))
            {
                result_string += "Your passwords do not match\n";
                result = false;
            }
            if (result)
                return true;
            else
            {
                CustomMessageBox.Show(result_string);
                return false;
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
