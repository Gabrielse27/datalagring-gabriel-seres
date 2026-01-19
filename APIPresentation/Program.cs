using Application;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Koppla in Databasen
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrera tjänsterna (Dependency Injection)
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<CourseService>();

// 3. Lägg till Swagger (Dokumentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();                   //AddOpenApi();

var app = builder.Build();

// Konfigurera Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();                         //app.MapOpenApi();
}

app.UseHttpsRedirection();

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

app.Run();