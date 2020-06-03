using GreenBox.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox.Models
{
    class SwagTestModel
    {

        public static int GetSwag(int project_id)
        {
            try
            {
                using(DataContext context = new DataContext())
                {
                    int sum = 0;
                    var elements = (from e in context.ElementLocations
                                    where e.ProjectId == project_id
                                    select e.ElementId);

                    foreach (var e in elements)
                        sum += GetK(e);

                    var zoom = (from p in context.Projects
                                where p.Id == project_id
                                select p.Zoom).FirstOrDefault();

                    var cost = (from p in context.Projects
                                where p.Id == project_id
                                select p.TotalCost).FirstOrDefault();

                    return Convert.ToInt32(sum / elements.Count() * 10 + zoom * cost / 10);

                }
            }
            catch { return 0; }
        }

        private static int GetK(int element_id)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    int elementK = (from e in context.Elements
                                    where e.Id == element_id
                                    select e.K).FirstOrDefault();
                    return elementK;
                }
            }
            catch { return 0; }
        }
    }
}
