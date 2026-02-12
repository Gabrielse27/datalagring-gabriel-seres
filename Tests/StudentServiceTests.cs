using Application;
using Domain;
using Moq;
using Xunit;
using Microsoft.Extensions.Caching.Memory;

namespace Tests
{
    public class StudentServiceTests
    {
        // 1. Vi behöver en variabel för Servicen vi ska testa
        private readonly StudentService _studentService;

       private readonly Mock<IStudentRepository> _mockRepository;

        private readonly Mock<IMemoryCache> _mockCache;
        public StudentServiceTests()
        {
            // A. Skapa den fejkade databas-kopplingen
            _mockRepository = new Mock<IStudentRepository>();

            _mockCache = new Mock<IMemoryCache>();

            // B. Skapa servicen och skicka in fejken istället för den riktiga databasen
            // Detta kallas "Dependency Injection" i testet

            _studentService = new StudentService(_mockRepository.Object, _mockCache.Object);
        }

        private object newMock<T>()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddStudent_ShouldCallRepository_WhenStudentIsValid()
        {
            // --- ARRANGE (Förberedelser) ---
            var newStudent = new Student { FirstName = "Anna Andersson", Age = 30 };

            // --- ACT (Utför handlingen) ---
            // Vi anropar metoden i servicen som vi vill testa
            _studentService.AddStudent(newStudent);

            _mockRepository.Verify(repo => repo.AddStudentAsync(It.IsAny<Student>()), Times.Once);
        }

    }
}
