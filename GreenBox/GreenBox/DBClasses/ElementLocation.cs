using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox.DBClasses
{
    public class ElementLocation
    {
        public int Id { get; set; }
        public int ElementId { get; set; }
        public int ProjectId { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public int Rotate { get; set; }

        
        public virtual Element Element { get; set; }
        public virtual Project Project { get; set; }
    }
}
