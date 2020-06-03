using GreenBox.Controls;
using GreenBox.Models;
using GreenBox.ViewModels.PagesViewModels;
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
    class GetHelpViewModel: INotifyPropertyChanged
    {
        private string emailMessage;
        public string EmailMessage
        {
            get { return emailMessage; }
            set
            {
                emailMessage = value;
                OnPropertyChanged("EmailMessage");
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

        private RelayCommand sendMessageCommand;
        public RelayCommand SendMessageCommand
        {
            get
            {
                return sendMessageCommand ??
                  (sendMessageCommand = new RelayCommand(obj =>
                  {
                      if (string.IsNullOrEmpty(ForgotPasswordModel.SendMessage("me", EmailMessage)))
                          CustomMessageBox.Show("Error");
                      else
                          CustomMessageBox.Show("Your message was sended, please wait for an answer.");
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
