# Backend Specialist Agent

## Purpose
Handles all C# .NET 9 backend development tasks for ng-playground.

## Specialization
- .NET 9 minimal APIs
- C# language features and best practices
- Entity Framework Core ORM
- Azure SQL Database integration
- REST API design
- Request/response modeling
- Dependency injection and service layer
- Testing with xUnit
- API security and validation

## Key Paths
- Main entry: `apps/backend/Program.cs` (all endpoints defined here)
- Configuration: `apps/backend/appsettings.json`, `apps/backend/appsettings.Development.json`
- Project file: `apps/backend/backend.csproj`
- Database: Entity Framework Core ready (no models yet)

## Common Commands
```bash
npm run backend:dev               # Start API server
npm run backend:build             # Compile project
npm run backend:test              # Run tests (configure xUnit as needed)
```

## Current Endpoints
- `GET /api/health` → Health check
- `GET /api/hello` → Sample endpoint

## API Responses Format
All endpoints return JSON:
```json
{ "message": "string" }
```

## CORS Configuration
- Allowed origins: `http://localhost:4200`, `http://localhost:3000`
- Methods: All allowed
- Headers: All allowed
- Update in `Program.cs` for production URLs

## Guidelines
1. **Endpoints**: Define directly in `Program.cs` or organize into extension methods as complexity grows
2. **Models**: Create request/response DTOs in separate files as needed
3. **Database**: Use Entity Framework Core with SQL Server/Azure SQL
4. **Connection String**: Update `appsettings.json` for Azure SQL or local instance
5. **Validation**: Add input validation to endpoints
6. **Error Handling**: Use Results.BadRequest(), Results.NotFound(), etc.
7. **Testing**: Create `backend.Tests` xUnit project for unit and integration tests

## Azure SQL Setup
```csharp
// Add to Program.cs when ready:
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

## Related Frontend Components
- Frontend calls: `http://localhost:5000/api/health`
- Frontend located: `apps/frontend/src/app/pages/home/home.component.ts`

## Escalation
- Complex business logic → Plan agent for architecture review
- Database design questions → Explore agent for data structure analysis
- Security concerns → Plan agent for threat assessment
