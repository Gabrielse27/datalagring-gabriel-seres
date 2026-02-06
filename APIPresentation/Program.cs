using Application;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


var builder = WebApplication.CreateBuilder(args);

// 1. Koppla in Databasen
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrera tjänsterna (Dependency Injection)
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<CourseService>();

// 3. Lägg till Swagger (Dokumentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<Domain.IStudentRepository, Infrastructure.StudentRepository>();
builder.Services.AddScoped<Application.StudentService>();


builder.Services.AddCors((options =>
{
    options.AddPolicy("AllowAll",  policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();

    });

}));

//AddOpenApi();

var app = builder.Build();

// --- SKAPA ENDPOINTS HÄR ---

 //Endpoint för att hämta alla studenter (Använder din Service med Caching!)

app.MapGet("/api/students", async (Application.StudentService service) =>
{
  var students = await service.GetAllStudents();
return Results.Ok(students);
});

// Endpoint för att lägga till en student


app.MapPost("/api/students", async (Application.StudentService service, Domain.Student student) =>
{
    await service.AddStudent(student);
    return Results.Created($"/api/students/{student.Id}", student);
});


// Konfigurera Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();                         //app.MapOpenApi();
}

app.UseHttpsRedirection();


app.UseCors("AllowAll");

// ---------------------------------------------------------
// HÄR ÄR DITT NYA MINIMAL API
// Vi kopplar adresserna direkt till CourseService
// ---------------------------------------------------------

// Hämta alla kurser
app.MapGet("/api/courses", async (CourseService service) =>
{
    return await service.GetAllCourses();
});

// Hämta en specifik kurs
app.MapGet("/api/courses/{id}", async (int id, CourseService service) =>
{
    var course = await service.GetCourseById(id);
    return course is not null ? Results.Ok(course) : Results.NotFound();
});

// VG-Endpoint: Sök med SQL
// URL blir t.ex: /api/students/search?name=Gabriel
app.MapGet("/api/students/search", async (Application.StudentService service, string name) =>
{
    var result = await service.SearchStudents(name);
    // Om vi hittar några studenter -> Returnera dem (200 OK)
    // Om listan är tom -> Returnera "Hittades inte" (404 Not Found)
    return result.Any() ? Results.Ok(result) : Results.NotFound("Inga studenter hittades med det namnet.");
});

// Endpoint: Uppdatera namn (Använder transaktion)

app.MapPut("/api/students/{id}/name", async (Application.StudentService service, int id, string firstName, string lastName) =>
{
    try
    {
        await service.UpdateStudentName(id, firstName, lastName);
        return Results.Ok("Uppdateringen lyckades! (Transaktionen committad)");
    }
    catch (Exception)
    {
        return Results.Problem("Något gick fel. Transaktionen har gjorts Rollback på.");
    }
});


// Skapa en ny kurs
app.MapPost("/api/courses", async (Course course, CourseService service) =>
{
    await service.AddCourse(course);
    return Results.Ok("Kursen skapad!");
});

// Uppdatera en kurs
app.MapPut("/api/courses", async (Course course, CourseService service) =>
{
    await service.UpdateCourse(course);
    return Results.Ok("Kursen uppdaterad!");
});



// Ta bort en kurs
app.MapDelete("/api/courses/{id}", async (int id, CourseService service) =>
{
    await service.DeleteCourse(id);
    return Results.Ok("Kursen borttagen!");
});

 //---Endpoint för att uppdatera hela studentobjektet

app.MapPut("/api/students/{id}", async (int id, Domain.Student student, Application.StudentService service) =>
{
    await service.UpdateStudent (student);
    return Results.Ok("Studenten Updaterad!");
});

app.MapDelete("/api/students/{id}", async (int id, IStudentRepository repo) =>
{
    // Vi försöker ta bort studenten
    var success = await repo.DeleteStudentAsync(id);

    if (success)
    {
        return Results.Ok("Studenten borttagen");
    }
    return Results.NotFound("Kunde inte hitta studenten");
});



app.Run();