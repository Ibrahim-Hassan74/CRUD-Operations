# CRUD Operation with ASP.NET Core MVC

## Overview
This project demonstrates a CRUD (Create, Read, Update, Delete) operation using ASP.NET Core MVC with Entity Framework Core (EF Core). It follows Clean Architecture principles, implements logging with Serilog, utilizes Tag Helpers, applies action filters, and includes unit testing using xUnit.

## Features
- **ASP.NET Core MVC** for building the web application.
- **Entity Framework Core (EF Core)** for database operations.
- **Clean Architecture** to separate concerns.
- **Tag Helpers** for cleaner and more maintainable Razor views.
- **Filters** for handling cross-cutting concerns such as logging and validation.
- **Serilog** for structured logging.
- **xUnit** for unit testing.

## Technologies Used
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **Serilog**
- **Tag Helpers**
- **Action Filters**
- **xUnit Testing Framework**

## Project Structure (Clean Architecture with Class Libraries)
```
📦 CRUDProject
 ┣ 📂 Application (Class Library for Business Logic)
 ┃ ┣ 📂 Interfaces
 ┃ ┣ 📂 Services
 ┣ 📂 Domain (Class Library for Entities)
 ┃ ┣ 📂 Entities
 ┣ 📂 Infrastructure (Class Library for Data and Repositories)
 ┃ ┣ 📂 Data
 ┃ ┣ 📂 Repositories
 ┃ ┣ 📂 Logging (Serilog Configuration)
 ┣ 📂 Web (ASP.NET Core MVC Application)
 ┃ ┣ 📂 Controllers
 ┃ ┣ 📂 Views
 ┃ ┣ 📂 wwwroot
 ┣ 📂 Tests (Class Library for Unit Tests)
 ┃ ┣ 📂 UnitTests (xUnit Tests)
```

## Setup Instructions
1. **Clone the repository:**
   ```sh
   git clone https://github.com/your-repo-url.git
   cd CRUDProject
   ```

2. **Update the database connection string**
   - Navigate to `appsettings.json` and modify the `ConnectionStrings` section to match your database.

3. **Apply Migrations:**
   ```sh
   dotnet ef database update
   ```

4. **Run the application:**
   ```sh
   dotnet run
   ```
   The application will be available at `https://localhost:5001/`.

## Logging with Serilog
- Logs are configured in `Program.cs` using Serilog.
- Logs are stored in a file and displayed in the console.
- Example log format:
  ```sh
  [2025-03-05 10:00:00] Information: User created successfully.
  ```

## Unit Testing with xUnit
- Navigate to the `Tests` project and run the tests:
  ```sh
  dotnet test
  ```

## Action Filters
- Custom action filters are used to handle logging and validation.
- Example: `LoggingActionFilter` logs request and response details.

## Contributing
Feel free to submit issues or pull requests to improve the project.

## License
This project is licensed under the MIT License.

