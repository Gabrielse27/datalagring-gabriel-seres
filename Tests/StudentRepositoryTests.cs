using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class StudentRepositoryTests
    {
        // Detta är ett INTEGRATIONSTEST
        // Vi testar att Repositoryt funkar mot en "riktig" databas (i minnet)
        [TestMethod]
        public async Task AddStudent_ShouldSaveToDatabase()
        {
            // 1. ARRANGE - Rigga en tom databas i minnet
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_AddStudent") // Unikt namn
                .Options;

            // Skapa contexten med dessa inställningar
            using var context = new AppDbContext(options);
            var repository = new StudentRepository(context);

            var newStudent = new Student
            {
                FirstName = "Test",
                LastName = "Integration",
                Email = "test@test.com"
            };

            // 2. ACT - Kör din riktiga metod
            await repository.AddStudentAsync(newStudent);

            // 3. ASSERT - Kolla om den faktiskt hamnade i databasen
            var studentInDb = await context.Students.FirstOrDefaultAsync(s => s.Email == "test@test.com");

            Assert.IsNotNull(studentInDb); // Den ska finnas!
            Assert.AreEqual("Test", studentInDb.FirstName); // Namnet ska stämma!
        }
    }
}