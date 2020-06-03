using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenBox.DBClasses;


namespace GreenBox.Models
{
    class ProjectModel
    {
        public static ObservableCollection<Project> GetProjects(int userId)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    var projects = from p in context.Projects
                                   join l in context.Links on p.Id equals l.ProjectId
                                   join u in context.Users on l.UserId equals u.Id where u.Id == userId
                                   orderby p.LastUpdate descending
                                   select new { id = p.Id, name = p.Name, lastUpdate = p.LastUpdate, covertype = p.CoverType, isFinished = p.IsFinished, totalCoast =  p.TotalCost, screenshot = p.Screenshot };

                    ObservableCollection<Project> result = new ObservableCollection<Project>();

                    foreach (var p in projects)
                    {
                        result.Add(new Project
                        {
                            Id = p.id,
                            Name = p.name,
                            LastUpdate = p.lastUpdate,
                            CoverType = p.covertype,
                            IsFinished = p.isFinished,
                            TotalCost = p.totalCoast,
                            Screenshot = p.screenshot
                        });
                    }

                    return result;
                }
                catch { return null; }
            }
        }

        public static bool DeleteProject(int project_id)
        {
            using(DataContext context = new DataContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var elementsLocations = from e in context.ElementLocations
                                 where e.ProjectId == project_id
                                 select e;

                        foreach (ElementLocation e in elementsLocations)
                            context.ElementLocations.Remove(e);

                        var links = from l in context.Links
                                    where l.ProjectId == project_id
                                    select l;

                        foreach (Link l in links)
                            context.Links.Remove(l);

                        Project project = (from p in context.Projects
                                           where p.Id == project_id
                                           select p).FirstOrDefault();

                        context.Projects.Remove(project);
                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}
