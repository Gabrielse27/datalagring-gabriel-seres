using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace Infrastructure
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }
        // Lägg till en ny kurs
        public async Task AddCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }
        // Ta bort en kurs
        public async Task DeleteCourse(int id)
        {
            var courseToDelete = await _context.Courses.FindAsync(id);
            if (courseToDelete != null )
            {
                _context.Courses.Remove(courseToDelete);
                await _context.SaveChangesAsync();
            }
        }
        // Hämta alla kurser
        public async Task<List<Course>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }
        // Hämta en kurs 
        public async Task<Course> GetCourseById (int id)
        {
            return await _context.Courses.FindAsync(id);
        }
        // Uppdatera en kurs
        public async Task UpdateCourse( Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
    }
}
