using System;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Lab01
{
    public class Person
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; }
        public byte[] Picture { get; set; }
    }
    public class PersonDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
    }

}

