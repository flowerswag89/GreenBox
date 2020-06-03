using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox.DBClasses
{
    public class Project
    {
        public int Id { get; set; }

        [StringLength(35)]
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }
        public string CoverType { get; set; }
        public bool IsFinished { get; set; }
        public double TotalCost { get; set; }

        [Column(TypeName = "image")]
        [MaxLength]
        public byte[]? Screenshot { get; set; }
        public double Zoom { get; set; }


        public virtual ICollection<Link> Links { get; set; }
        public virtual ICollection<ElementLocation> ElementLocations { get; set; }
    }
}
