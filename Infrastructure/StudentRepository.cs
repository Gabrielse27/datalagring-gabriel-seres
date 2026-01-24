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
            return await _context.Students.ToListAsync();
        }

        public async Task AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

        }
    }
}
