using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenBox.DBClasses;

namespace GreenBox
{
    public class DataContext:DbContext
    {
        public DataContext() : base("DefaultConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<ElementLocation> ElementLocations { get; set; }
    }
}
