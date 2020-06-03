using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GreenBox.Controls;
using GreenBox.DBClasses;
using GreenBox.Models;
using GreenBox.ViewModels.PagesViewModels;
using GreenBox.Views;
using GreenBox.Views.Pages;

namespace GreenBox.ViewModels
{
    class CreateProjectViewModel: INotifyPropertyChanged
    {

        private int currentUserId;
        public int CurrentUserId
        {
            get { return currentUserId; }
            set
            {
                currentUserId = value;
                OnPropertyChanged("CurrentUserId");
            }
        }

        private Element selectedElement;
        public Element SelectedElement
        {
            get { return selectedElement; }
            set
            {
                selectedElement = value;
                OnPropertyChanged("SelectedElement");
            }
        }


        public ObservableCollection<Element> Covers { get; set; }

        public CreateProjectViewModel(int userid)
        {
            CurrentUserId = userid;
            Covers = CreateProjectModel.GetCovers(CurrentUserId);
            Name = "";
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

        private RelayCommand createProjectCommand;
        public RelayCommand CreateProjectCommand
        {
            get
            {
                return createProjectCommand ??
                    (createProjectCommand = new RelayCommand(obj =>
                    {
                        if (Validator())
                        {
                            if (CreateProjectModel.NameVerificator(Name, currentUserId))
                            {
                                bool result = CreateProjectModel.ProjectCreater(Name, SelectedElement.Name, CurrentUserId);
                                if (result == true)
                                {
                                    MainWindow mv = MainWindow.Initialize(CurrentUserId, new ConstructorPage(CreateProjectModel.ProjectIdReturner(Name), CurrentUserId));
                                    (obj as Window).Close();
                                    return;
                                }
                                else
                                    CustomMessageBox.Show("We have some problem, try anymore");
                            }
                            else
                            {
                                CustomMessageBox.Show("Project with this name is exsist");
                            }

                        }
                        else
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
                    }));
            }
        }

        private bool Validator()
        {
            bool result = true;
            string result_string = "";
            if(string.IsNullOrEmpty(Name) || Name.Length > 30)
            {
                result = false;
                result_string += "Enter name of project (max length:30)\n"; 
            }
            if(SelectedElement == null)
            {
                result = false;
                result_string += "Cover type not choiced\n";
            }


            if (result == false)
                CustomMessageBox.Show(result_string);
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
