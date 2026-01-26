using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;




namespace Domain
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task AddStudentAsync(Student student);

        Task<List<Student>> GetStudentBySqlAsync(string searchName);
    }
}
