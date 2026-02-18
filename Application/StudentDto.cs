using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public class StudentDto
    {
         public int Id { get; set; }

       
         public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public List<string> Courses { get; set; } = new List<string>();
    }
}
