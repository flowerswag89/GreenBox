using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenBox.DBClasses;

namespace GreenBox.Models
{
    class CreateProjectModel
    {
        public static ObservableCollection<Element> GetCovers(int userId)
        {
            ObservableCollection<Element> result = new ObservableCollection<Element>();
            using(DataContext context = new DataContext())
            {
                try
                {
                    var covers = from c in context.Elements
                                 where c.Type == "Cover"
                                 select new { id = c.Id, name = c.Name, info = c.Info, cost = c.Cost, topImage = c.TopImage, sideImage = c.SideImage, size = c.Size, type = c.Type, userId = c.UserId };


                    foreach(var c in covers)
                    {
                        result.Add(new Element
                        {
                            Id = c.id,
                            Name = c.name,
                            Info = c.info,
                            Cost = c.cost,
                            TopImage = c.topImage,
                            SideImage = c.sideImage,
                            Size = c.size,
                            Type = c.type,
                            UserId = c.userId
                        });
                    }
                }
                catch { return null; }
                return result;
            }

        }

        public static int ProjectIdReturner(string project_name)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    int project_id = (from p in context.Projects
                                      where p.Name == project_name
                                      select p.Id).First();
                    return project_id;
                }
                catch { return 0; }
            }
        }


        public static bool NameVerificator(string project_name, int user_id)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    int? project_id = (from p in context.Projects
                                      where p.Name == project_name
                                      select p.Id).FirstOrDefault();

                    if (project_id != null)
                    {
                        var project = (from p in context.Projects
                                       join l in context.Links on p.Id equals l.ProjectId
                                       join u in context.Users on l.UserId equals u.Id
                                       where p.Id == project_id && user_id == u.Id
                                       select p).FirstOrDefault();
                        if (project != null)
                            return false;
                        else
                            return true;
                    }
                    else
                        return false;
                }
                catch { return false; }
            }
        }


        public static bool ProjectCreater(string name, string cover, int user_id)
        {
            bool result = true;

            using(DataContext context = new DataContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Projects.Add(new Project
                        {
                            Name = name,
                            LastUpdate = DateTime.Now,
                            CoverType = cover,
                            IsFinished = false,
                            TotalCost = 0,
                            Zoom = 1
                        });
                        context.SaveChanges();

                        int project_id = (from p in context.Projects
                                  where p.Name == name
                                  select p.Id).First();

                        context.Links.Add(new Link()
                        {
                            UserId = user_id,
                            ProjectId = project_id
                        });

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        transaction.Rollback();
                    }
                    return result;
                }
            }
        }
    }
}
