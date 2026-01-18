using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface ICourseRepository
    {

        Task AddCourse(Course course);
        Task DeleteCourse(int id);
        Task<List<Course>> GetAllCourses();
        Task<Course> GetCourseById(int id);
        Task UpdateCourse(Course course);
    }
}
