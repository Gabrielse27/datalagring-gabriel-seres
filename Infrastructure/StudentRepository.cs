using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Student>> GetAllStudentsAsync()
        {
            // Hämtar listan från databasen
            return await _context.Students
                .Include(s => s.Enrollments)
                .ToListAsync();
        }


        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return false; // Hittades inte -> Returnera False
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true; // Borttagen -> Returnera True
        }




        public async Task AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

        }

        public async Task <List<Student>>GetStudentBySqlAsync(string searchName)
        {
         // Entity Framework Core används
            var result = await _context.Students
                // Använder Rå SQL-fråga för att hämta studenter baserat på sökord
                .FromSqlRaw("SELECT * FROM Students WHERE FirstName LIKE {0} OR LastName LIKE {0}", "%" + searchName + "%")
                .Include(s => s.Enrollments)
                .ToListAsync();
                return result;

        }

        // VG-KRAV: Transaktionshantering med Rollback
        public async Task UpdateStudentNameAsync(int id, string firstName, string lastName)
        {
            // 1. Starta en transaktion (Allt eller inget!)
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Leta upp studenten
                var student = await _context.Students.FindAsync(id);

                if (student != null)
                {
                    // Gör ändringen
                    student.FirstName = firstName;
                    student.Lastname = lastName;

                    // Spara ändringen i minnet (men transaktionen är fortfarande öppen)
                    await _context.SaveChangesAsync();

                    // 2. Allt gick bra? -> COMMIT (Lås fast ändringarna permanent)
                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                // 3. Något gick fel? -> ROLLBACK (Ångra exakt allt som gjordes)
                await transaction.RollbackAsync();

                // Kasta felet vidare
                throw;
            }
        }

        

    }
}
