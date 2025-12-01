# AdventureWorks Sample - C# Windows Forms Teaching Application

## Description

Windows Forms .NET 8.0 application for managing the AdventureWorks database.
This project is designed for teaching purposes in a C# programming course.

**Main features:**
- Uses **Dapper** as micro-ORM (no Entity Framework)
- Clean architecture with separation between UI and data access
- Extensively commented code to facilitate learning
- Three CRUD forms for main tables

## Managed Tables

1. **Production.Product** - Product master data management
2. **Sales.SalesOrderHeader** - Sales order management
3. **Sales.SalesOrderDetail** - Order line management

## Requirements

- .NET 8.0 SDK
- SQL Server with AdventureWorks database installed
- Visual Studio 2022 or VS Code (optional)

## How to Run

### 1. Clone the repository
```bash
git clone https://github.com/spaghettidba/AdventureWorksSample.git
cd AdventureWorksSample/AdventureWorksApp
```

### 2. Configure the database connection

Edit the `appsettings.json` file with your connection string:

```json
{
  "ConnectionStrings": {
    "AdventureWorks": "Server=YOUR_SERVER;Database=AdventureWorks2019;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

**Or** enter the connection string directly in the application at first startup.

### 3. Build and run
```bash
dotnet restore
dotnet build
dotnet run
```

## Project Structure

```
AdventureWorksApp/
├── appsettings.json          # Configuration
├── Program.cs                # Entry point
├── Models/                   # Model classes (POCO)
│   ├── Product.cs
│   ├── SalesOrderHeader.cs
│   └── SalesOrderDetail.cs
├── DataAccess/              # Data access layer
│   ├── DbConnectionFactory.cs
│   ├── ProductRepository.cs
│   ├── SalesOrderHeaderRepository.cs
│   └── SalesOrderDetailRepository.cs
└── Forms/                   # Windows Forms
    ├── MainForm.cs/.Designer.cs
    ├── ProductForm.cs/.Designer.cs
    ├── SalesOrderHeaderForm.cs/.Designer.cs
    └── SalesOrderDetailForm.cs/.Designer.cs
```

## Patterns Used

### Repository Pattern
Each table has its own repository that encapsulates CRUD operations:
- `GetAllAsync()` - Reads all records
- `GetByIdAsync(id)` - Reads a single record
- `InsertAsync(entity)` - Inserts a new record
- `UpdateAsync(entity)` - Updates an existing record
- `DeleteAsync(id)` - Deletes a record

### Dapper
Dapper is a micro-ORM that extends `IDbConnection` with mapping methods:

```csharp
// Example query with Dapper
using var connection = DbConnectionFactory.CreateConnection();
var products = await connection.QueryAsync<Product>(
    "SELECT * FROM Production.Product WHERE Name LIKE @Name",
    new { Name = "%bike%" }
);
```

## Teaching Notes

1. **Separation of concerns**: UI and data logic are separated
2. **Async/Await**: All DB operations are asynchronous to avoid blocking the UI
3. **Parameterized queries**: Prevent SQL injection
4. **Comments**: The code is extensively commented

## Connection String Examples

**Windows Authentication (recommended):**
```
Server=localhost;Database=AdventureWorks2019;Integrated Security=True;TrustServerCertificate=True
```

**SQL Server Express:**
```
Server=localhost\SQLEXPRESS;Database=AdventureWorks2019;Integrated Security=True;TrustServerCertificate=True
```

**SQL Authentication:**
```
Server=localhost;Database=AdventureWorks2019;User Id=sa;Password=YourPassword;TrustServerCertificate=True
```

## License

This project is released for teaching purposes.