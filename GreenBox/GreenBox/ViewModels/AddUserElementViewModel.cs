using GreenBox.Controls;
using GreenBox.Models;
using GreenBox.ViewModels.PagesViewModels;
using Microsoft.Win32;
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
    class AddUserElementViewModel : INotifyPropertyChanged
    {
        private string topImage;
        public string TopImage
        {
            get { return topImage; }
            set
            {
                topImage = value;
                OnPropertyChanged("TopImage");
            }
        }

        private string sideImage;
        public string SideImage
        {
            get { return sideImage; }
            set
            {
                sideImage = value;
                OnPropertyChanged("SideImage");
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

        private string info;
        public string Info
        {
            get { return info; }
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }

        private double cost;
        public double Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged("Cost");
            }
        }

        private int size;
        public int Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }

        private int userId;

        public AddUserElementViewModel(int user_id)
        {
            userId = user_id;
            TopImage = "..\\..\\Resources\\DefaultImage\\browse_top_image.jpg";
            SideImage = "..\\..\\Resources\\DefaultImage\\browse_side_image.jpg";
        }

        private bool Validator()
        {
            string result_string = "";
            bool result = true;

            if ((TopImage == "..\\..\\Resources\\DefaultImage\\browse_top_image.jpg"))
            {
                result_string += "Insert Top Image\n";
                result = false;
            }
            if ((SideImage == "..\\..\\Resources\\DefaultImage\\browse_side_image.jpg"))
            {
                result_string += "Insert Side Image\n";
                result = false;
            }
            if ((string.IsNullOrEmpty(Name) || Name.Length > 30))
            {
                result_string += "Check  Russian mame (max length: 30)\n";
                result = false;
            }
            if (string.IsNullOrEmpty(Info))
            {
                Info = "0_0";
            }
            try
            {
                if (Cost == 0 || Convert.ToDouble(Cost) > 999)
                {

                    result = false;
                    result_string += "Enter cost of element (max:999)\n";
                }
            }
            catch { result = false; result_string = ("You cost is not a number\n"); }

            try
            {
                if (Size == 0 || Convert.ToInt32(Size) > 300)
                {

                    result = false;
                    result_string += "Enter size of element (max:300)\n";
                }
            }
            catch { result = false; result_string = ("You size is not a number\n"); }

            if (result == true)
            {
                result = AddUserElementModel.NameImageVerificator(Name, AddUserElementModel.GetBytes(TopImage), AddUserElementModel.GetBytes(SideImage));
                if (result == false)
                    result_string += "Element whith this name is exists";
            }
            if (result)
                return true;
            else
            {
                CustomMessageBox.Show(result_string);
                return false;
            }
        }


        private RelayCommand addElementCommand;
        public RelayCommand AddElementCommand
        {
            get
            {
                return addElementCommand ??
                  (addElementCommand = new RelayCommand(obj =>
                  {
                      if (Validator())
                      {
                          if (AddUserElementModel.ElementInsert(AddUserElementModel.GetBytes(TopImage), AddUserElementModel.GetBytes(SideImage), Name, Info, Cost, Size, userId))
                          {
                              CustomMessageBox.Show("Element was added");
                              (obj as Window).Close();
                          }
                          else
                              CustomMessageBox.Show("Error");
                      }
                      return;
                  }));
            }
        }



        private RelayCommand addTopImageCommand;
        public RelayCommand AddTopImageCommand
        {
            get
            {
                return addTopImageCommand ??
                  (addTopImageCommand = new RelayCommand(obj =>
                  {

                      try
                      {
                          OpenFileDialog dialog = new OpenFileDialog();
                          dialog.Filter = "txt files (*.jpg)|*.jpg|All files (*.*)|*.*";
                          dialog.InitialDirectory = @"C:\";
                          dialog.Title = "Please select an image file to encrypt.";

                          Nullable<bool> result = dialog.ShowDialog();

                          // Process save file dialog box results
                          if (result == true)
                          {
                              TopImage = dialog.FileName;
                          }
                      }
                      catch { return; }

                      return;
                  }));
            }
        }

        private RelayCommand addSideImageCommand;
        public RelayCommand AddSideImageCommand
        {
            get
            {
                return addSideImageCommand ??
                  (addSideImageCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          OpenFileDialog dialog = new OpenFileDialog();
                          dialog.Filter = "txt files (*.jpg)|*.jpg|All files (*.*)|*.*";
                          dialog.InitialDirectory = @"C:\";
                          dialog.Title = "Please select an image file to encrypt.";

                          Nullable<bool> result = dialog.ShowDialog();

                          // Process save file dialog box results
                          if (result == true)
                          {
                              SideImage = dialog.FileName;
                          }
                      }
                      catch { return; }
                      return;
                  }));
            }
        }
        private RelayCommand closeWindowCommad;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return closeWindowCommad ??
                  (closeWindowCommad = new RelayCommand(obj =>
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
