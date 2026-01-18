using Application;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace APIPresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Detta gör att adressen blir /api/courses
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;

        // Här ber vi om att få vår Service (som du precis kopplade i Program.cs)
        public CoursesController(CourseService courseService)
        {
            _courseService = courseService;
        }

        // Hämta alla kurser
        [HttpGet]
        public async Task<ActionResult<List<Course>>> GetAll()
        {
            var courses = await _courseService.GetAllCourses();
            return Ok(courses);
        }

        // Hämta en specifik kurs
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetById(int id)
        {
            var course = await _courseService.GetCourseById(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        // Skapa en ny kurs
        [HttpPost]
        public async Task<ActionResult> Create(Course course)
        {
            await _courseService.AddCourse(course);
            return Ok("Kursen skapad!");
        }

        // Uppdatera en kurs
        [HttpPut]
        public async Task<ActionResult> Update(Course course)
        {
            await _courseService.UpdateCourse(course);
            return Ok("Kursen uppdaterad!");
        }

        // Ta bort en kurs
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _courseService.DeleteCourse(id);
            return Ok("Kursen borttagen!");
        }
    }
}
