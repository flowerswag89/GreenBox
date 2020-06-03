using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.ObjectModel;
using GreenBox.DBClasses;
using GreenBox.ViewModels;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace GreenBox.Models
{
    class ConstructorModel
    {
        public static List<Element> GetElementCollection(string type, int user_id)
        {
            using (DataContext context = new DataContext())
            {

                try
                {
                    List<Element> result = new List<Element>();

                    if (type != "User")
                    {
                        var elements = from e in context.Elements
                                       where e.Type == type
                                       select new { id = e.Id, name = e.Name, info = e.Info, cost = e.Cost, topImage = e.TopImage, sideImage = e.SideImage, size = e.Size, type = e.Type, userId = e.UserId };
                        

                        foreach (var e in elements)
                        {
                            result.Add(new Element
                            {
                                Id = e.id,
                                Name = e.name,
                                Info = e.info,
                                Cost = e.cost,
                                TopImage = e.topImage,
                                SideImage = e.sideImage,
                                Size = e.size,
                                Type = e.type,
                                UserId = e.userId
                            });
                        }
                    }
                    else
                    {
                        var elements = from e in context.Elements
                                       where e.Type == type && e.UserId == user_id
                                       select new { id = e.Id, name = e.Name, info = e.Info, cost = e.Cost, topImage = e.TopImage, sideImage = e.SideImage, size = e.Size, type = e.Type, userId = e.UserId };
                        

                        foreach (var e in elements)
                        {
                            result.Add(new Element
                            {
                                Id = e.id,
                                Name = e.name,
                                Info = e.info,
                                Cost = e.cost,
                                TopImage = e.topImage,
                                SideImage = e.sideImage,
                                Size = e.size,
                                Type = e.type,
                                UserId = e.userId
                            });
                        }
                    }

                    return result;
                }
                catch { return null; }
            }
        }

        public static bool SavePolyanaCollection(ObservableCollection<PolyanaElement> polyana1, int project_id, double total)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    var savedCollection = from s in context.ElementLocations
                                          where s.ProjectId == project_id
                                          select new { pId = s.ProjectId, eId = s.ElementId, x = s.XPosition, y = s.YPosition , rot = s.Rotate};

                    ObservableCollection<PolyanaElement> savedPolyana = new ObservableCollection<PolyanaElement>();
                    foreach (var s in savedCollection)
                        savedPolyana.Add(new PolyanaElement(s.eId, GetImage(s.eId), s.x, s.y, GetSize(s.eId), s.rot));

                    ObservableCollection<PolyanaElement> buffer = new ObservableCollection<PolyanaElement>();


                    foreach(var p in polyana1)
                    {
                        if (!savedPolyana.Contains<PolyanaElement>(p))
                            savedPolyana.Add(p);
                    }

                    foreach(var s in savedPolyana)
                    {
                        if (!polyana1.Contains<PolyanaElement>(s))
                            buffer.Add(s);
                    }

                    foreach (var b in buffer)
                        savedPolyana.Remove(b);

                    var cleanCollection = from c in context.ElementLocations
                                          where c.ProjectId == project_id
                                          select c;

                    foreach (var c in cleanCollection)
                        context.ElementLocations.Remove(c);

                    foreach (var s in savedPolyana)
                        context.ElementLocations.Add(new ElementLocation() 
                        {
                            ElementId = s.ElementId,
                            ProjectId = project_id,
                            XPosition = s.X + s.Size/2,
                            YPosition = s.Y + s.Size/2,
                            Rotate = s.Rotate
                        });

                    Project prj = (from p in context.Projects
                                   where p.Id == project_id
                                   select p).FirstOrDefault();

                    prj.TotalCost = total;


                    context.SaveChanges();

                    return true;
                }
                catch { return false; }
            }
        }

        public static PolyanaElement GetPolyanaElement(int element_id, System.Windows.Point point)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    byte[] element_image = (from e in context.Elements
                                            where e.Id == element_id
                                            select e.TopImage).First();

                    int size = (from e in context.Elements
                                where e.Id == element_id
                                select e.Size).First();

                    PolyanaElement p = new PolyanaElement(element_id, element_image, point.X, point.Y, size, 0);

                    return p;
                }
                catch { return null; }
            }
        }

        public static void ProjectDateUpdate(int project_id, double zoom)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    Project project = (from p in context.Projects
                                       where p.Id == project_id
                                       select p).FirstOrDefault();
                    project.LastUpdate = DateTime.Now;
                    project.Zoom = zoom;
                    context.SaveChanges();
                }
                catch { }
            }
        }

        public static byte[] GetCover(int project_id)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    string cover_name = (from p in context.Projects
                                         where p.Id == project_id
                                         select p.CoverType).First();

                    byte[] cover = (from p in context.Elements
                                         where p.Name == cover_name
                                         select p.TopImage).First();


                    return cover;
                }
                catch { return null; }
            }
        }

        public static double GetZoom(int project_id)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    double zoom = (from p in context.Projects
                                         where p.Id == project_id
                                         select p.Zoom).First();

                    return zoom;
                }
                catch { return 1; }
            }
        }

        public static ObservableCollection<PolyanaElement> GetPolyanaCollections(int project_id)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    ObservableCollection<PolyanaElement> result = new ObservableCollection<PolyanaElement>();

                    var elements = from e in context.ElementLocations
                                   where e.ProjectId == project_id
                                   select new { id = e.ElementId, x = e.XPosition, y = e.YPosition , rot = e.Rotate};

                    foreach(var e in elements)
                    {
                        result.Add(new PolyanaElement(e.id, GetImage(e.id), e.x, e.y, GetSize(e.id), e.rot));
                    }

                    return result;
                }
                catch { return null; }
            }
        }

        private static byte[] GetImage(int element_id)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    byte[] image = (from e in context.Elements
                                    where e.Id == element_id
                                    select e.TopImage).First();
                    return image;
                }
                catch { return null; }
            }
        }

        private static int GetSize(int element_id)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    int size = (from e in context.Elements
                                    where e.Id == element_id
                                    select e.Size).First();
                    return size;
                }
                catch { return 0; }
            }
        }

        public static double GetCost(int element_id)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    double price = (from e in context.Elements
                                    where e.Id == element_id
                                    select e.Cost).FirstOrDefault();
                    return price;
                }
                catch { return 0; }
            }
        }

        public static double GetProjectCost(ObservableCollection<PolyanaElement> polyana)
        {
            double cost = 0;
            foreach (var p in polyana)
                cost += GetCost(p.ElementId);
            return cost;
        }

        public static string GetProjectName(int project_id)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    string project_name = (from p in context.Projects
                                          where p.Id == project_id
                                          select p.Name).FirstOrDefault();
                    return project_name;
                }
                catch { return ""; }
            }
        }

        public static bool GetFinish(int project_id)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    bool isFinished = (from p in context.Projects
                                           where p.Id == project_id
                                           select p.IsFinished).FirstOrDefault();
                    return isFinished;
                }
                catch { return false; }
            }
        }


        public static void InsertProjectImage(int project_id, byte[] image)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    Project project = (from p in context.Projects
                                       where p.Id == project_id
                                       select p).FirstOrDefault();
                    project.Screenshot = image;
                    context.SaveChanges();
                }
                catch { }
            }
        }

        public static void SaveFinish(int project_id, bool isFinished)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    Project project = (from p in context.Projects
                                       where p.Id == project_id
                                       select p).FirstOrDefault();
                    project.IsFinished = isFinished;
                    context.SaveChanges();
                }
                catch { }
            }
        }
    }
}
