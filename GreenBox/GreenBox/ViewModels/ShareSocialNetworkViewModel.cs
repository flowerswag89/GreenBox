using GreenBox.ViewModels.PagesViewModels;
using GreenBox.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GreenBox.ViewModels
{
    class ShareSocialNetworkViewModel: INotifyPropertyChanged
    {
        private byte[] currentImage;
        public byte[] CurrentImage
        {
            get { return currentImage; }
            set
            {
                currentImage = value;
                OnPropertyChanged("CurrentImage");
            }
        }
        public ShareSocialNetworkViewModel(byte[] image)
        {
            CurrentImage = image;
        }


        private RelayCommand shareVkCommand;
        public RelayCommand ShareVkCommand
        {
            get
            {
                return shareVkCommand ??
                    (shareVkCommand = new RelayCommand(obj =>
                    {
                        VkWindow vkw = new VkWindow(CurrentImage);
                        vkw.ShowDialog();
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
