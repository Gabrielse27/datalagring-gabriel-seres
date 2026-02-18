using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;



namespace Application
{
    public class StudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IMemoryCache _cache;

        // Här injectar vi både databaskopplingen och cache-minnet
        public StudentService(IStudentRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<StudentDto>> GetAllStudents()
        {
            const string cacheKey = "studentsListDto";

            // 1. Kolla cachen (Har vi färdiga DTO:er i minnet?)
            if (!_cache.TryGetValue(cacheKey, out List<StudentDto> studentsDto))
            {
                // 2. Om INTE: Hämta från databasen...
                var studentsFromDb = await _repository.GetAllStudentsAsync();

                // ...OCH gör om dem till DTO:er direkt (Min kod)
                studentsDto = studentsFromDb.Select(s => new StudentDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Age = s.Age,
                    Courses = s.Enrollments.Select(e => 
                           string.IsNullOrWhiteSpace(e.Course.Location)
                           ? e.Course.Title
                           : e.Course.Title + "("+ e.Course.Location +")"
                           ).ToList(),
                }).ToList();

                // 3. Spara listan i minnet i 2 minuter
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(5));

                _cache.Set(cacheKey, studentsDto, cacheOptions);
            }

            return studentsDto;
        }


        //  prova DTO
        // --- VG-DELEN: Hämta med Caching ---//
        //
       // public async Task<List<Student>> GetAllStudents()
     //   {
       //     const string cacheKey = "studentsList";

            // 1. Kolla om listan redan finns i minnet (Cache)
        //   if (!_cache.TryGetValue(cacheKey, out List<Student> students))
        //    {
                // 2. Om den INTE finns, hämta från databasen
         //       students = await _repository.GetAllStudentsAsync();

                // 3. Spara listan i minnet i 2 minuter
           //     var cacheOptions = new MemoryCacheEntryOptions()
           //         .SetSlidingExpiration(TimeSpan.FromMinutes(2));

          //      _cache.Set(cacheKey, students, cacheOptions); 
         //   }

      //     return students; 
       // }
// prova DTO   //

        // Lägg till student
        public async Task AddStudent(Student student)
        {
            await _repository.AddStudentAsync(student);

            // VIKTIGT: När vi lägger till nytt måste vi rensa gamla cachen
            // så att den nya studenten syns nästa gång vi hämtar listan.
            _cache.Remove("studentsListDto");
        }

        public async Task<List<Student>> SearchStudents(string searchName)
        {
            return await _repository.GetStudentBySqlAsync(searchName);
        }

        public async Task UpdateStudentInfo (int id , string firstName , string lastName, int age)
        {
            await _repository.UpdateStudentAsync(id, firstName, lastName, age);
        }


        public async Task UpdateStudent(Student student)
        {
            // Logik för att uppdatera i databasen via repot
            // T.ex: await _studentRepository.UpdateStudentAsync(student);


            // 1. Skicka datan vidare till databasen (nu med Age!)
            await _repository.UpdateStudentAsync(student.Id, student.FirstName, student.LastName, student.Age);

            // 2. VIKTIGT: Rensa cachen så att Swagger visar rätt nästa gång vi hämtar listan
            _cache.Remove("studentsListDto");
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {

            _cache.Remove("studentsListDto");

            // Ropa på repositoryt som gör jobbet
            return await _repository.DeleteStudentAsync(id);
        }


    }
}