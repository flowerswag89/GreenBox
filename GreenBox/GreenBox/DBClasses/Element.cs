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
    public class Element
    {
        public int Id { get; set; }

        [StringLength(35)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Info { get; set; }
        public double Cost { get; set; }

        [Column(TypeName = "image")]
        [MaxLength]
        public byte[]? TopImage { get; set; }

        [Column(TypeName = "image")]
        [MaxLength]
        public byte[]? SideImage { get; set; }

        public int Size { get; set; }
        [StringLength(35)]
        public string Type{ get; set; }
        public int? UserId { get; set; }
        public int K { get; set; }


        public virtual User User { get; set; }
        public virtual ICollection<ElementLocation> ElementLocations { get; set; }
    }
}
