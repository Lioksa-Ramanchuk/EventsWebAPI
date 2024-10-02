# Event and Participant Management API

  This project is an ASP.NET Core Web API for managing events and participants. It supports functionalities such as creating, updating, deleting, and retrieving event and participant information. It includes automatic database migration setup using LocalDB and integrates with Swagger for API documentation.

## Prerequisites

  - [.NET SDK 8.0 or later](https://dotnet.microsoft.com/download/dotnet/8.0)
  - [LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15)

## Installation

   Clone the repository to your local machine:

   ```bash
   git clone https://github.com/Lioksa-Ramanchuk/EventsWebAPI
   cd EventsWebAPI
   ```

## Configuration

  Navigate to the ```Events``` folder where the startup project is located:

  ```bash
  cd src/Events
  ```

  You can configure the database connection string and other settings in ```appsettings.json```. The default connection string for LocalDB is set as follows:

  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EventsDb;Trusted_Connection=True;"
  }
  ```

## Running the Application

  Execute the following command to run the application from the ```Events``` folder:
  
  ```bash
  dotnet run
  ```

  ...or the following command from the installation folder:

  ```bash
  dotnet run --project src/Events
  ```

## Swagger UI

  Once the application is running, you can access the Swagger UI to view and test the API endpoints at the following URLs:
  - https://localhost:5001/swagger
  - http://localhost:5000/swagger (this will redirect to HTTPS)

## Database migration

  The database will be created automatically from the migrations found in ```src/Events.Infrastructure/Data/Migrations```.

## Testing

  The test project ```Events.Tests``` located in the ```test``` folder runs unit and integration tests using a temporary LocalDB instance with a GUID name, which is deleted after the tests complete. You can run the tests executing the following command from the installation folder:

  ```bash
  dotnet test test/Events.Tests
  ```
