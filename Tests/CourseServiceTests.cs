using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Application;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;







namespace Tests
{
    [TestClass]
    public class CourseServiceTests
    {
        [TestMethod]
        public async Task GetAllCourses_ShouldReturnListOfCourses()
        {
            // Arrange = Förberedelse
            // Vi skapar en fejkad databaskoppling med Moq
            var mockRepo = new Mock<ICourseRepository>();

            // Vi säger åt fejken: "Om någon ber om alla kurser, ge dem den här listan"
            mockRepo.Setup(repo => repo.GetAllCourses())
            .ReturnsAsync(new List<Course>
             {
               new Course { Id = 1, Title = "Matte 1", Description = "Grundläggande", Teacher = "Anna", Price = 1000m },
               new Course { Id = 2, Title = "Engelska 5", Description = "Nybörjare", Teacher = "Bertil", Price = 1500m} 
             });

            // Vi skapar vår Service och skickar in fejken istället för riktiga databasen
            var service = new CourseService(mockRepo.Object);

            // Act = Utförande
            // Vi kör metoden vi vill testa
            var result = await service.GetAllCourses();

            // Assert = Verifiering
            // vi kollar så att vi fick tillbaka 2 kurser som vi förväntade oss
            Assert.IsNotNull (result);
            Assert.AreEqual (2, result.Count);  // Kollar att det är 2 kurser i listan
            Assert.AreEqual ("Matte 1", result[0].Title);  // Kollar att första kursen har rätt titel



        }
    }
}
