# InvestList API

This is the backend API for the InvestList application, built with .NET 8 and following Clean Architecture principles.

## Project Structure

```
InvestList.API/
├── Controllers/         # API endpoints
├── Services/           # Business logic services
├── Models/             # DTOs and ViewModels
├── Middleware/         # Custom middleware
└── DTOs/              # Data Transfer Objects
```

## Dependencies

- .NET 8
- Entity Framework Core
- AutoMapper
- JWT Authentication
- Swagger/OpenAPI

## Setup

1. Clone the repository
2. Restore NuGet packages
3. Update the connection string in `appsettings.json`
4. Run database migrations (if needed)
5. Start the application

## Development

To run the application in development mode:

```bash
dotnet run --environment Development
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## API Documentation

Swagger documentation is available at `/swagger` when running in development mode.

## Authentication

The API uses JWT authentication. To authenticate:

1. Get a token from the authentication endpoint
2. Include the token in the Authorization header: `Bearer <token>`

## Environment Configuration

- Development: `appsettings.Development.json`
- Production: `appsettings.json`

## Notes

- The API is designed to work with the InvestList React frontend
- It reuses business logic from the existing InvestList application
- Follows Clean Architecture principles
- Uses dependency injection for loose coupling 