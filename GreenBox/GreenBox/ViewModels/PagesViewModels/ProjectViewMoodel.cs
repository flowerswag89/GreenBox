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
using System.Windows.Input;
using GreenBox.Models;
using GreenBox.DBClasses;
using GreenBox.Views;
using GreenBox.Controls;
using GreenBox.Views.Pages;
using System.Text.RegularExpressions;

namespace GreenBox.ViewModels.PagesViewModels
{
    public class ProjectViewMoodel:INotifyPropertyChanged
    {


        private int currentUserId;
        public int CurrentUserId
        {
            get { return currentUserId; }
            set
            {
                currentUserId = value;
                OnPropertyChanged("SelectedProject");
            }
        }


        private int projectsNumber;
        public int ProjectsNumber
        {
            get { return projectsNumber; }
            set
            {
                projectsNumber = value;
                OnPropertyChanged("ProjectsNumber");
            }
        }

        private string searchString;
        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                OnPropertyChanged("SearchString");
                SearchCommand.Execute(21);
            }
        }

        private byte[] projectScreen;
        public byte[] ProjectScreen
        {
            get { return projectScreen; }
            set
            {
                projectScreen = value;
                OnPropertyChanged("ProjectScreen");
            }
        }

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Project> ResultProjects { get; set; }

        public ProjectViewMoodel(int userid)
        {
            CurrentUserId = userid;
            Projects = ProjectModel.GetProjects(currentUserId);
            ProjectsNumber = Projects.Count();
            ResultProjects = new ObservableCollection<Project>();
            foreach (var p in Projects)
                ResultProjects.Add(p);
        }

        private Project selectedProject;

        public Project SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                OnPropertyChanged("SelectedProject");
                if (SelectedProject != null)
                    ProjectScreen = SelectedProject.Screenshot;
                else
                    projectScreen = null;
            }
        }


        private RelayCommand searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return searchCommand ??
                    (searchCommand = new RelayCommand(obj =>
                    {
                        string pattern = @$"^{SearchString}\w*";
                        if (string.IsNullOrEmpty(SearchString))
                        {
                            ResultProjects.Clear();
                            foreach (var p in Projects)
                                ResultProjects.Add(p);
                        }
                        else
                        {
                            ResultProjects.Clear();
                            foreach (var p in Projects)
                            {
                                if (Regex.IsMatch(p.Name, pattern) || Regex.IsMatch(p.Name.ToLower(), pattern) || Regex.IsMatch(p.Name.ToUpper(), pattern))
                                    ResultProjects.Add(p);
                            }
                        }
                    }));
            }
        }


        private RelayCommand openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                    (openCommand = new RelayCommand(obj =>
                    {
                        MainWindow mv = MainWindow.Initialize(CurrentUserId, new ConstructorPage(SelectedProject.Id, CurrentUserId));
                    }));
            }
        }

        private RelayCommand swagTestCommand;
        public RelayCommand SwagTestCommand
        {
            get
            {
                return swagTestCommand ??
                    (swagTestCommand = new RelayCommand(obj =>
                    {
                        SwagTestWindow sw;
                        if (SelectedProject == null)
                            CustomMessageBox.Show("Choose project");
                        else
                        {
                            sw = new SwagTestWindow(SelectedProject.Id);
                            sw.ShowDialog();

                        }
                    }));
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
                        var messageBoxResult = CustomMessageBoxYesNo.Show("Delete project", "Are you sure?",MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            if (ProjectModel.DeleteProject(SelectedProject.Id))
                                CustomMessageBox.Show("Project was deleted");
                            else
                                CustomMessageBox.Show("Error");

                            Projects.Clear();
                            ObservableCollection<Project> buffer = ProjectModel.GetProjects(CurrentUserId);
                            foreach (var b in buffer)
                                Projects.Add(b);

                            ResultProjects.Clear();
                            buffer = ProjectModel.GetProjects(CurrentUserId);
                            foreach (var b in buffer)
                                ResultProjects.Add(b);

                            ProjectsNumber = buffer.Count();
                        }
                    }));
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        CreateProjectWindow cpw = new CreateProjectWindow(CurrentUserId);
                        cpw.ShowDialog();
                    }));
            }
        }

        private RelayCommand viewPreviwCommand;
        public RelayCommand ViewPreviwCommand
        {
            get
            {
                return viewPreviwCommand ??
                    (viewPreviwCommand = new RelayCommand(obj =>
                    {
                        CustomMessageBox.Show((obj as TextBlock).Text);
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
