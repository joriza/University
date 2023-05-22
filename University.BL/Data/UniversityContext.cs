using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.BL.Models;

namespace University.BL.Data
{
    public class UniversityContext : DbContext
    {
        private static UniversityContext universityContext = null;
        public UniversityContext() : base("UniversityContext")
        {
            // Server=HCI-NOTE227\SQLEXPRESS;Database=University;Integrated Security=True
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public static UniversityContext Create()
        {
            if (universityContext == null)
                universityContext = new UniversityContext();

            return universityContext;
            //return new UniversityContext();
        }
    }
}
