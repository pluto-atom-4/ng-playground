# Copilot Instructions for ng-playground

This is a **monorepo** containing an Angular 19 frontend and .NET 9 backend with shared commands at the root.

## Project Structure

```
ng-playground/
├── apps/
│   ├── frontend/    # Angular 19 standalone components (pnpm workspace)
│   ├── backend/     # .NET 9 minimal APIs
│   └── ...
├── package.json     # Root scripts for controlling both frontend and backend
└── pnpm-workspace.yaml
```

## Build, Test, and Lint Commands

**All commands run from the repository root.**

### Frontend (Angular 19)

```bash
npm run frontend:dev          # Start dev server (http://localhost:4200, auto-reload)
npm run frontend:build        # Production build → apps/frontend/dist
npm run frontend:lint         # Run ESLint on TypeScript
npm run frontend:test         # Run Jasmine tests in watch mode
npm run frontend:test:single  # Run Jasmine tests once (CI mode)
```

**Running a single test file:** Use the interactive Karma browser runner or grep the spec file name:
```bash
npm run frontend:test         # Opens browser, use --include flag for filtering
```

### Backend (.NET 9)

```bash
npm run backend:dev      # Start API server (http://localhost:5000, auto-reload with dotnet watch)
npm run backend:build    # Compile C# project
npm run backend:test     # Run xUnit tests (currently no tests configured)
npm run backend:restore  # Restore NuGet dependencies
```

### Installation

```bash
npm run install          # Installs all dependencies (frontend + backend)
```

This runs `pnpm install` for the frontend and `dotnet restore` for the backend.

## Architecture & Key Patterns

### Frontend: Angular 19 Standalone Components

**Key locations:**
- `apps/frontend/src/main.ts` — Bootstrap application
- `apps/frontend/src/app/app.config.ts` — Global providers (routing, HTTP client config)
- `apps/frontend/src/app/app.routes.ts` — Route definitions
- `apps/frontend/src/app/` — All components, services, and pages organized by feature

**Patterns:**
- **Standalone components**: All components use `standalone: true`; no NgModule
- **HTTP client**: Injected as `HttpClient` with typed request/response models
- **Routing**: Using `provideRouter()` in app.config.ts; components use `RouterOutlet`
- **Styling**: SCSS with component encapsulation (`encapsulation: ViewEncapsulation.ShadowDom` or scoped styles)
- **RxJS**: Observables for async operations, use `async` pipe in templates

**Conventions:**
- Generate components/services via `ng generate`: `ng generate component pages/feature-name`
- Service names: `*.service.ts`
- Component selector prefix: `app-` (configured in `angular.json`)
- Test files: `*.spec.ts` (Jasmine/Karma)

### Backend: .NET 9 Minimal APIs

**Key locations:**
- `apps/backend/Program.cs` — API startup, endpoint definitions, middleware, CORS
- `apps/backend/appsettings.json` — Configuration (connection strings, logging)
- `apps/backend/appsettings.Development.json` — Local development overrides
- `apps/backend/Models/` — Data models and entities
- `apps/backend/Data/` — DbContext and migrations
- `apps/backend/Services/` — Business logic and services
- `apps/backend/DTOs/` — Data transfer objects for API responses

**Patterns:**
- **Minimal APIs**: Use `app.MapGet()`, `app.MapPost()`, etc. directly in Program.cs or call extension methods for organization
- **CORS**: Configured in Program.cs for `localhost:4200` and `localhost:3000`
- **Database**: Entity Framework Core 9.0 with SQL Server (Azure SQL ready)
- **Service injection**: Register services in `builder.Services` before building the app
- **Configuration**: Access config via `builder.Configuration` or `IConfiguration` in services

**Conventions:**
- Endpoint routes start with `/api/`
- Add `.WithName()` and `.WithOpenApi()` to enable OpenAPI documentation
- Return `Results.Ok()`, `Results.BadRequest()`, `Results.NotFound()` for responses
- Models in `Models/`; DbContext in `Data/`; business logic in `Services/`

### Frontend-Backend Communication

- Frontend runs on **localhost:4200** (development)
- Backend runs on **localhost:5000** (development)
- Frontend makes HTTP requests to `http://localhost:5000/api/*`
- CORS is configured in backend Program.cs to allow these requests

## Key Conventions

### Environment Setup

**Frontend environment handling:**
- Development: `ng serve` (dev server with live reload)
- Build: Configured in `angular.json` with production optimizations

**Backend environment handling:**
- Development: `ASPNETCORE_ENVIRONMENT=Development` (set automatically in npm scripts)
- Uses `appsettings.Development.json` for local overrides
- Database connection string should be in `appsettings.json` or user secrets in development

### Monorepo Workspace

- **Package manager**: pnpm (not npm)
- **Workspace configuration**: `pnpm-workspace.yaml` defines `apps/frontend` as a package
- **Backend**: Not in pnpm workspace (uses dotnet directly)
- **Root scripts**: Use `npm run` at repository root; the scripts use `-F frontend` to filter pnpm commands

### API Patterns

**Backend endpoint structure:**
```csharp
app.MapGet("/api/resource", () => {
    // Logic
    return Results.Ok(data);
})
.WithName("GetResource")
.WithOpenApi();
```

**Frontend HTTP calls:**
```typescript
constructor(private http: HttpClient) {}

fetchData() {
  return this.http.get<DataModel>('/api/resource').subscribe(data => {
    console.log(data);
  });
}
```

HTTP client is configured with a base URL in `app.config.ts` to point to `http://localhost:5000` during development.

### Styling Conventions

- **Frontend**: SCSS (component-scoped via `styleUrls` or inline `styles`)
- **Global styles**: `apps/frontend/src/styles.scss`
- **No CSS framework pre-configured**: Add Angular Material, Bootstrap, Tailwind as needed

### Testing Conventions

**Frontend (Jasmine/Karma):**
- Test files: `*.spec.ts` colocated with component/service
- Run tests: `npm run frontend:test` (watch) or `npm run frontend:test:single` (CI)
- Mock HTTP: Use `HttpClientTestingModule` from `@angular/common/http/testing`

**Backend (xUnit):**
- No tests currently configured
- To add: Create `backend.Tests/` project with `dotnet new xunit`
- Naming: `*.Tests.cs`

## Common Development Scenarios

### Adding a New Frontend Component

```bash
cd apps/frontend
ng generate component pages/my-feature
```

### Adding a New Backend Endpoint

Edit `apps/backend/Program.cs` and add:
```csharp
app.MapGet("/api/my-endpoint", () => Results.Ok(new { message = "Hello" }))
    .WithName("MyEndpoint")
    .WithOpenApi();
```

### Calling Backend from Frontend

Inject `HttpClient` in component:
```typescript
constructor(private http: HttpClient) {}

ngOnInit() {
  this.http.get<{ message: string }>('/api/my-endpoint').subscribe(
    response => console.log(response.message)
  );
}
```

### Setting Up a New Database

1. Update `appsettings.json` with connection string for Azure SQL or local SQL Server
2. Create a `DbContext` class in `apps/backend/Data/`:
   ```csharp
   public class AppDbContext : DbContext {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
       public DbSet<MyModel> MyModels => Set<MyModel>();
   }
   ```
3. Add migration: `cd apps/backend && dotnet ef migrations add InitialCreate`
4. Apply migration: `dotnet ef database update`

### Running Both Frontend and Backend

Open two terminal tabs:
```bash
# Terminal 1
npm run frontend:dev

# Terminal 2
npm run backend:dev
```

The frontend will automatically make requests to the backend at `localhost:5000`.

## File Structure Recap

```
apps/frontend/
├── src/
│   ├── app/
│   │   ├── pages/          # Page components
│   │   ├── components/     # Reusable components
│   │   ├── services/       # HTTP and business logic services
│   │   ├── app.routes.ts   # Routing configuration
│   │   └── app.config.ts   # Global providers
│   ├── main.ts             # Bootstrap
│   ├── index.html
│   └── styles.scss         # Global styles
├── angular.json
├── tsconfig.json
└── package.json

apps/backend/
├── Program.cs              # All endpoints and configuration
├── Models/                 # Entity and data models
├── Data/                   # DbContext and migrations
├── Services/               # Business logic
├── DTOs/                   # Request/response models
├── appsettings.json        # Configuration
├── appsettings.Development.json
└── backend.csproj
```

## References

For more architecture details, see [CLAUDE.md](../CLAUDE.md) which includes multi-agent workflow information.
