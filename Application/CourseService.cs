using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Threading.Tasks;



namespace Application
{
    public class CourseService
    {
        private readonly ICourseRepository _repository;

        // Här ber vi om att få "verktyget" (Repository) automatiskt
        public CourseService(ICourseRepository repository)
        {
            _repository = repository;
        }

        public async Task <List<Course>> GetAllCourses()
        {
            return await _repository.GetAllCourses();
        }

        public async Task AddCourse(Course course)
        {
            await _repository.AddCourse(course);
        }

        public async Task<Course> GetCourseById(int id)
        {
            //await _repository.GetCourseById(id);
            return await _repository.GetCourseById(id);
        }

        public async Task UpdateCourse(Course course)
        {
            await _repository.UpdateCourse(course);
        }
        public async Task DeleteCourse(int id)
        {
            await _repository.DeleteCourse(id);
        }

    }
}
