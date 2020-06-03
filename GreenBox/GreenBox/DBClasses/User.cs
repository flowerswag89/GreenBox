using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace GreenBox.DBClasses
{
    public class User
    {
        public int Id { get; set; }

        [StringLength(35)]
        public string Surname { get; set; }

        [StringLength(35)]
        public string Name { get; set; }

        [StringLength(35)]
        public string Email { get; set; }

        [StringLength(35)]
        public string Username { get; set; }

        [MaxLength(100)]
        public byte[] Password { get; set; }

        [Column(TypeName = "image")]
        [MaxLength]
        public byte[]? Icon { get; set; }
        public virtual ICollection<Link> Links { get; set; } 
        public virtual ICollection<Element> Elements { get; set; }
    }
}
