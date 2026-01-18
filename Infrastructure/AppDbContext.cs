using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    // Den här klassen är själva "kopplingen" till databasen
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // Här listar vi alla tabeller som ska finnas i databasen
        public DbSet<Course> Courses { get; set; }

    }
}
