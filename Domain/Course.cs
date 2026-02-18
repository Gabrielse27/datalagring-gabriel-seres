using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Course
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public string Teacher { get; set; }

        public decimal Price { get; set; }

        public string Location { get; set; } = "Distans";

        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
