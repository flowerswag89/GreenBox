using GreenBox.Controls;
using GreenBox.DBClasses;
using GreenBox.Models;
using GreenBox.ViewModels.PagesViewModels;
using GreenBox.Views;
using GreenBox.Views.Pages;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GreenBox.ViewModels
{
    class ConstructorViewModel : INotifyPropertyChanged
    {

        private double projectCost = 0;
        private Point point = new Point();
        private double zoom = 1;
        ConstructorPage pG;

        private Stack<ObservableCollection<PolyanaElement>> undoStack;
        private Stack<ObservableCollection<PolyanaElement>> redoStack;


        private string projectCostString;
        public string ProjectCostString
        {
            get { return projectCostString; }
            set
            {
                projectCostString = value;
                OnPropertyChanged("ProjectCostString");
            }
        }

        private bool isFinished;
        public bool IsFinished
        {
            get { return isFinished; }
            set
            {
                isFinished = value;
                OnPropertyChanged("IsFinished");
            }
        }

        private ScaleTransform zoomProperty;
        public ScaleTransform ZoomProperty
        {
            get { return zoomProperty; }
            set
            {
                zoomProperty = value;
                OnPropertyChanged("ZoomProperty");
            }
        }

        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        private bool IsSelected = false;
        private bool IsLocal = false;

        private int userId;

        private Cursor currentCursor;
        public Cursor CurrentCursor
        {
            get { return currentCursor; }
            set
            {
                currentCursor = value;
                OnPropertyChanged("CurrentCursor");
            }
        }

        private byte[] projectCover;
        public byte[] ProjectCover
        {
            get { return projectCover; }
            set
            {
                projectCover = value;
                OnPropertyChanged("ProjectCover");
            }
        }



        public ObservableCollection<PolyanaElement> PolyanaCollection { get; set; }

        private PolyanaElement CurrentPolyanaElement = null;


        private List<Element> elementsCollection;
        public List<Element> ElementsCollection
        {
            get { return elementsCollection; }
            set
            {
                elementsCollection = value;
                OnPropertyChanged("ElementsCollection");
            }
        }


        private int currentProjectId;
        public int CurrentProjectId
        {
            get { return currentProjectId; }
            set
            {
                currentProjectId = value;
                OnPropertyChanged("CurrentProjectId");
            }
        }

        private GreenBox.Models.MenuItem selectedMenuItem;
        public GreenBox.Models.MenuItem SelectedMenuItem
        {
            get { return selectedMenuItem; }
            set
            {
                selectedMenuItem = value;
                OnPropertyChanged("SelectedMenuItem");
                ElementsCollection = ConstructorModel.GetElementCollection(value.Type, userId);
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
                if(CurrentPolyanaElement != null)
                {
                    PolyanaElement buffer = CurrentPolyanaElement;
                    PolyanaCollection.Remove(CurrentPolyanaElement);
                    buffer.BorderThinckessProperty = 0;
                    PolyanaCollection.Add(buffer);
                    CurrentPolyanaElement = null;
                    IsLocal = false;
                }
                IsSelected = true;
                CurrentCursor = Cursors.SizeAll;
            }
        }


        public ObservableCollection<GreenBox.Models.MenuItem> MenuItemCollection { get; set; }

        public ConstructorViewModel(int project_id, int user_id, ConstructorPage p)
        {

            ProjectName = ConstructorModel.GetProjectName(project_id);
            CurrentProjectId = project_id;
            userId = user_id;
            ProjectCover = ConstructorModel.GetCover(CurrentProjectId);
            MenuItemCollection = GreenBox.Models.MenuItem.GetMenuItemCollection();
            SelectedMenuItem = MenuItemCollection[0];
            PolyanaCollection = ConstructorModel.GetPolyanaCollections(CurrentProjectId);

            projectCost = ConstructorModel.GetProjectCost(PolyanaCollection);
            ProjectCostString = projectCost + "$";

            zoom = ConstructorModel.GetZoom(project_id);
            ZoomProperty = new ScaleTransform(zoom, zoom, 50, 50);

            pG = p;

            IsFinished = ConstructorModel.GetFinish(project_id);

            undoStack = new Stack<ObservableCollection<PolyanaElement>>();
            redoStack = new Stack<ObservableCollection<PolyanaElement>>();
        }

        private RelayCommand addCanvasCommand;
        public RelayCommand AddCanvasCommand
        {
            get
            {
                return addCanvasCommand ??
                  (addCanvasCommand = new RelayCommand(obj =>
                  {
                      if(IsSelected == true && IsLocal == false) // Клик при добавлении на поляну IsSelected присваивается true при выборе типа элемента
                      {

                          ObservableCollection<PolyanaElement> buffer2 = new ObservableCollection<PolyanaElement>();
                          foreach (var p in PolyanaCollection)
                              buffer2.Add(p);

                          undoStack.Push(buffer2);

                          point = Mouse.GetPosition(obj as Canvas);
                          CurrentPolyanaElement = ConstructorModel.GetPolyanaElement(SelectedElement.Id, point);
                          PolyanaCollection.Add(CurrentPolyanaElement);

                          projectCost += ConstructorModel.GetCost(CurrentPolyanaElement.ElementId);
                          ProjectCostString = projectCost + "$";

                          CurrentPolyanaElement.BorderThinckessProperty = 0;
                          CurrentPolyanaElement = null;
                          IsSelected = false;
                          CurrentCursor = Cursors.Arrow;
                      }
                      else if(IsSelected == false && IsLocal == false) // клик для выбора элемента на поляне
                      {

                          point = Mouse.GetPosition(obj as Canvas);
                          ReverseCollection(PolyanaCollection);
                          foreach(var p in PolyanaCollection)
                          {
                              if ((point.X - p.Size/2 > p.X - p.Size/2 && point.X - p.Size / 2 < p.X + p.Size/2 )&&(point.Y - p.Size / 2 > p.Y - p.Size / 2 && point.Y - p.Size / 2 < p.Y + p.Size / 2))
                              {
                                  CurrentPolyanaElement = p;
                                  PolyanaCollection.Remove(p);
                                  CurrentPolyanaElement.BorderThinckessProperty = 2;
                                  ReverseCollection(PolyanaCollection);
                                  PolyanaCollection.Add(CurrentPolyanaElement);
                                  IsSelected = true;
                                  IsLocal = true;
                                  CurrentCursor = Cursors.SizeAll;
                                  return;
                              }
                          }
                          ReverseCollection(PolyanaCollection);
                      }
                      else if(IsSelected == true && IsLocal == true) // клик для установки выбранного элемента в новое место
                      {

                          ObservableCollection<PolyanaElement> buffer3 = new ObservableCollection<PolyanaElement>();
                          foreach (var p in PolyanaCollection)
                              buffer3.Add(new PolyanaElement(p.ElementId, p.Image, p.X + p.Size/2, p.Y + p.Size/2, p.Size, p.Rotate));


                          undoStack.Push(buffer3);

                          point = Mouse.GetPosition(obj as Canvas);
                          PolyanaElement buffer = CurrentPolyanaElement;
                          PolyanaCollection.Remove(CurrentPolyanaElement);
                          buffer.X = point.X - CurrentPolyanaElement.Size / 2;
                          buffer.Y = point.Y - CurrentPolyanaElement.Size / 2;
                          buffer.BorderThinckessProperty = 0;
                          PolyanaCollection.Add(buffer);
                          CurrentPolyanaElement = null;
                          IsSelected = false;
                          IsLocal = false;
                          CurrentCursor = Cursors.Arrow;
                      }
                  }));
            }
        }

        private void ReverseCollection(ObservableCollection<PolyanaElement> p)
        {
            ObservableCollection<PolyanaElement> buffer = new ObservableCollection<PolyanaElement>(p.Reverse());
            PolyanaCollection.Clear();
            foreach (var b in buffer)
                PolyanaCollection.Add(b);
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      ConstructorModel.SavePolyanaCollection(PolyanaCollection, CurrentProjectId, projectCost);
                      ConstructorModel.ProjectDateUpdate(CurrentProjectId, zoom);
                      ConstructorModel.InsertProjectImage(CurrentProjectId, MakePhoto(obj));
                      MainWindow mw = MainWindow.Initialize(userId, pG);
                      ConstructorModel.SaveFinish(CurrentProjectId,IsFinished);

                      Popup.Show("Saved", mw);

                      undoStack.Clear();
                      redoStack.Clear();

                      return;
                  }));
            }
        }

        private RelayCommand showInfoCommand;
        public RelayCommand ShowInfoCommand
        {
            get
            {
                return showInfoCommand ??
                  (showInfoCommand = new RelayCommand(obj =>
                  {

                      point = Mouse.GetPosition(obj as Canvas);


                      ReverseCollection(PolyanaCollection);
                      foreach (var p in PolyanaCollection)
                      {
                          if ((point.X - p.Size / 2 > p.X - p.Size / 2 && point.X - p.Size / 2 < p.X + p.Size / 2) && (point.Y - p.Size / 2 > p.Y - p.Size / 2 && point.Y - p.Size / 2 < p.Y + p.Size / 2))
                          {
                              Element element = HomeModel.GetElement(p.ElementId);
                              if (element.Type == "Tree" || element.Type == "Bush" || element.Type == "Flower" || element.Type == "User")
                              {
                                  ElementInfoCard eic = new ElementInfoCard(element, userId);
                                  eic.ShowDialog();
                              }
                              return;
                          }
                      }
                      ReverseCollection(PolyanaCollection);
                      return;
                  }));
            }
        }

        private RelayCommand zoomPlusCommand;
        public RelayCommand ZoomPlusCommand
        {
            get
            {
                return zoomPlusCommand ??
                  (zoomPlusCommand = new RelayCommand(obj =>
                  {
                      zoom += 0.1;
                      ZoomProperty = new ScaleTransform(zoom, zoom, 50, 50);
                      MainWindow mw = MainWindow.Initialize(userId, pG);
                      Popup.Show($"{zoom * 100}%", mw);
                      return;
                  }));
            }
        }

        private RelayCommand zoomMinusCommand;
        public RelayCommand ZoomMinusCommand
        {
            get
            {
                return zoomMinusCommand ??
                  (zoomMinusCommand = new RelayCommand(obj =>
                  {
                      zoom -= 0.1;
                      ZoomProperty = new ScaleTransform(zoom, zoom, 50, 50);
                      MainWindow mw = MainWindow.Initialize(userId, pG);
                      Popup.Show($"{zoom * 100}%", mw);
                      return;
                  }));
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
                      AddUserElementWindow auew = new AddUserElementWindow(userId);
                      auew.ShowDialog();
                      return;
                  }));
            }
        }


        private RelayCommand undoCommand;
        public RelayCommand UndoCommand
        {
            get
            {
                return undoCommand ??
                  (undoCommand = new RelayCommand(obj =>
                  {

                      if (undoStack.Count() != 0)
                      {
                          ObservableCollection<PolyanaElement> buffer1 = new ObservableCollection<PolyanaElement>();
                          foreach (var p in undoStack.Peek())
                          {
                              p.BorderThinckessProperty = 0;
                              buffer1.Add(p);
                          }

                          undoStack.Pop();

                          ObservableCollection<PolyanaElement> buffer2 = new ObservableCollection<PolyanaElement>();
                          foreach (var p in PolyanaCollection)
                          {
                              p.BorderThinckessProperty = 0;
                              buffer2.Add(p);
                          }


                          redoStack.Push(buffer2);

                          PolyanaCollection.Clear();

                          projectCost = 0;

                          foreach (var m in buffer1)
                          {
                              PolyanaCollection.Add(m);
                              projectCost += ConstructorModel.GetCost(m.ElementId);
                          }
                          ProjectCostString = projectCost + "$";

                      }

                      return;
                  }));
            }
        }


        private RelayCommand redoCommand;
        public RelayCommand RedoCommand
        {
            get
            {
                return redoCommand ??
                  (redoCommand = new RelayCommand(obj =>
                  {
                      if(redoStack.Count() != 0)
                      {
                          ObservableCollection<PolyanaElement> buffer1 = new ObservableCollection<PolyanaElement>();
                          foreach (var p in redoStack.Peek())
                              buffer1.Add(p);

                          redoStack.Pop();

                          ObservableCollection<PolyanaElement> buffer2 = new ObservableCollection<PolyanaElement>();
                          foreach (var p in PolyanaCollection)
                              buffer2.Add(p);


                          undoStack.Push(buffer2);

                          PolyanaCollection.Clear();

                          projectCost = 0;

                          foreach (var m in buffer1)
                          {
                              PolyanaCollection.Add(m);
                              projectCost += ConstructorModel.GetCost(m.ElementId);
                          }
                          ProjectCostString = projectCost + "$";
                      }
                      return;
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
                      if (IsSelected == true && IsLocal == true)
                      {
                          ObservableCollection<PolyanaElement> buffer2 = new ObservableCollection<PolyanaElement>();
                          foreach (var p in PolyanaCollection)
                              buffer2.Add(p);

                          undoStack.Push(buffer2);

                          PolyanaCollection.Remove(CurrentPolyanaElement);
                          projectCost -= ConstructorModel.GetCost(CurrentPolyanaElement.ElementId);
                          if (PolyanaCollection.Count() == 0)
                              projectCost = 0;
                          if (PolyanaCollection.Count() == 1 && ConstructorModel.GetCost(PolyanaCollection[0].ElementId) == 0)
                              projectCost = 0;
                          ProjectCostString = projectCost + "$";
                          CurrentPolyanaElement = null;
                          IsSelected = false;
                          IsLocal = false;

                          CurrentCursor = Cursors.Arrow;
                      }
                      return;
                  }));
            }
        }

        private RelayCommand makePhotoAndSahreCommand;
        public RelayCommand MakePhotoAndShareCommand
        {
            get
            {
                return makePhotoAndSahreCommand ??
                  (makePhotoAndSahreCommand = new RelayCommand(obj =>
                  {
                      ShareSocialNetworkWindow ssnw = new ShareSocialNetworkWindow(MakePhoto(obj));
                      ssnw.ShowDialog();

                      return;
                  }));
            }
        }


        private RelayCommand turnLeftCommand;
        public RelayCommand TurnLeftCommand
        {
            get
            {
                return turnLeftCommand ??
                  (turnLeftCommand = new RelayCommand(obj =>
                  {
                      if (IsSelected == true && IsLocal == true)
                      {
                          PolyanaElement buffer = CurrentPolyanaElement;
                          PolyanaCollection.Remove(CurrentPolyanaElement);
                          buffer.Rotate -= 90;
                          CurrentPolyanaElement = buffer;
                          PolyanaCollection.Add(CurrentPolyanaElement);
                      }
                      return;
                  }));
            }
        }

        private RelayCommand turnRightCommand;
        public RelayCommand TurnRightCommand
        {
            get
            {
                return turnRightCommand ??
                  (turnRightCommand = new RelayCommand(obj =>
                  {
                      if (IsSelected == true && IsLocal == true)
                      {
                          PolyanaElement buffer = CurrentPolyanaElement;
                          PolyanaCollection.Remove(CurrentPolyanaElement);
                          buffer.Rotate += 90;
                          CurrentPolyanaElement = buffer;
                          PolyanaCollection.Add(CurrentPolyanaElement);
                      }

                      return;
                  }));
            }
        }


        private byte[] MakePhoto(object obj)
        {
            double d = 0;

            if (zoom > 0.9 && zoom <= 1.1)
                d = 96;
            else if (zoom <= 0.9 && zoom >= 0.8)
                d = 128;
            else if (zoom > 1.1 && zoom < 1.2)
                d = 78;
            else if (zoom <= 0.8)
                d = 168;
            else if (zoom >= 1.2)
                d = 78;

            int height = (int)(obj as ItemsControl).ActualHeight;
            int width = (int)(obj as ItemsControl).ActualWidth;
            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, d, d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(obj as ItemsControl);
            //endcode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();
            return ms.ToArray();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
