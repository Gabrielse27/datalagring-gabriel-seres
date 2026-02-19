# School Management System - Backend API

This is the Backend API for the School Management System, built with ASP.NET Core Web API and Entity Framework Core.

The project follows Clean Architecture principles and implements advanced database features such as transaction handling with rollback support.

##  Architecture

The solution is structured according to Domain-Driven Design (DDD) and Clean Architecture:

* API (Presentation): Contains the minimal API endpoints and Dependency Injection setup.
* Application: Contains business logic, DTOs, and Services ( `StudentService`).
* Domain: Contains the entities (e.g., `Student`, `Course`) and repository interfaces.
* Infrastructure: Handles database connections, EF Core migrations, and repository implementations.
* Tests: Contains Tests  to verify business logic and repository functions.

##  Key Features

* CRUD Operations: Full management of Students and Courses.
* Entity Framework Core: Uses Code First approach with Migrations.
* Transaction Management :
    * Implements secure transaction handling.
    * Rollback support: If an error occurs during name updates, the database rolls back changes to ensure data integrity.
* Testing: Includes Tests for CourseService , StudentRepository and StudentService.

##  Technologies

* Framework: .NET 10 (ASP.NET Core)
* Database: SQL Servee via  Entity Framework Core
* Testing: xUnit, Moq
* Tools: Swagger ,  Postman , for Api Testing. 
* Sql Server Object Explorer   (dbo.Courses) (dbo.Students)


##  How to Run Locally

Follow these steps to set up the backend server:
Click on Play ▶️.
Localhost https://localhost:7005/swagger

## 1. Conditions
Ensure you have the following installed:
* .NET SDK 10.0   https://dotnet.microsoft.com/en-us/download
* SQL Server (LocalDB)

