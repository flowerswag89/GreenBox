using GreenBox.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox.Models
{
    class ElementInfoCardModel
    {
        public static bool DeleteUserElement(int element_id)
        {
            using(DataContext context = new DataContext())
            {
                try
                {
                    var elements = from e in context.ElementLocations
                                   where e.Id == element_id
                                   select new { id = e.Id, projectid = e.ProjectId, elementid = e.ElementId, x = e.XPosition, y = e.YPosition, rot = e.Rotate };

                    if (elements != null)
                    {
                        List<ElementLocation> elementsCollection = new List<ElementLocation>();

                        foreach (var e in elements)
                        {
                            elementsCollection.Add(new ElementLocation
                            {
                                Id = e.id,
                                ProjectId = e.projectid,
                                ElementId = e.elementid,
                                XPosition = e.x,
                                YPosition = e.y,
                                Rotate = e.rot
                            });
                        }

                        foreach (var e in elementsCollection)
                            context.ElementLocations.Remove(e);
                    }


                    Element element = (from e in context.Elements
                                       where e.Id == element_id
                                       select e).FirstOrDefault();

                    context.Elements.Remove(element);


                    context.SaveChanges();
                    return true;
                }
                catch { return false; }
            }
        }
    }
}
