# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**ng-playground** is a monorepo full-stack application with:
- **Frontend**: Angular 19 standalone components (apps/frontend)
- **Backend**: .NET 9 minimal APIs (apps/backend)
- **Database**: Azure SQL Database (configuration-ready)
- **Package Manager**: pnpm workspaces

## Repository Structure

```
ng-playground/
├── apps/
│   ├── frontend/           # Angular application
│   │   ├── src/
│   │   │   ├── app/       # Components, routing, services
│   │   │   ├── index.html
│   │   │   └── main.ts
│   │   ├── angular.json
│   │   ├── tsconfig.json
│   │   └── package.json
│   ├── backend/            # .NET 9 minimal API
│   │   ├── Program.cs      # Entry point with endpoint configuration
│   │   ├── backend.csproj
│   │   └── appsettings.json
├── package.json            # Root workspace configuration
└── pnpm-workspace.yaml
```

## Quick Start

### Installation

```bash
# Install all dependencies (frontend + backend packages)
npm run install

# Or separately:
pnpm install                    # Frontend dependencies
cd apps/backend && dotnet restore
```

### Development

```bash
# Start Angular dev server (http://localhost:4200)
npm run frontend:dev

# Start .NET backend (http://localhost:5000)
npm run backend:dev

# The frontend health check component will call GET /api/health on the backend
```

### Build & Test

```bash
# Frontend
npm run frontend:build           # Production build
npm run frontend:lint           # Run ESLint
npm run frontend:test           # Run Jasmine tests
npm run frontend:test:watch     # Run tests in watch mode
npm run frontend:test:single    # Run tests once

# Backend
npm run backend:build           # Compile C# project
npm run backend:test            # Run xUnit tests (once configured)
```

## Architecture

### Frontend (Angular 19)

**Key Files:**
- `src/main.ts` — Bootstrap application
- `src/app/app.config.ts` — Providers (routing, HTTP client)
- `src/app/app.routes.ts` — Route definitions
- `src/app/pages/home/home.component.ts` — Example component with HTTP calls

**Features:**
- Standalone components (no NgModule)
- Typed HTTP client with RxJS observables
- SCSS styling with component encapsulation
- Routing with RouterOutlet
- CORS-enabled to communicate with backend on localhost:5000

**Adding Components:**
```bash
cd apps/frontend
ng generate component pages/new-page
ng generate service services/api
```

### Backend (.NET 9)

**Key Files:**
- `Program.cs` — Application startup, endpoint definitions, CORS configuration
- `appsettings.json` — Configuration (connection strings, logging)
- `backend.csproj` — Project file with NuGet dependencies

**Current Endpoints:**
- `GET /api/health` — Returns `{ message: "Backend is healthy!..." }`
- `GET /api/hello` — Returns `{ message: "Hello from C# .NET 9..." }`

**Features:**
- Minimal APIs (no controllers)
- CORS enabled for localhost:4200 and :3000
- Entity Framework Core configured (not yet integrated with database)
- Ready for Azure SQL Database connection

**Adding Endpoints:**
```csharp
app.MapPost("/api/data", (DataModel input) =>
{
    // Logic here
    return Results.Ok(result);
});
```

**Database Setup (Azure SQL):**
1. Update `appsettings.json` with your Azure connection string
2. Create DbContext class inheriting from `EntityFrameworkCore.DbContext`
3. Add migrations: `dotnet ef migrations add InitialCreate`
4. Apply migrations: `dotnet ef database update`

## Development Workflow

### Making Changes

**Frontend:**
- Modify components in `src/app/`
- Hot reload is automatic via `ng serve`
- Tests run alongside with `npm run frontend:test:watch`

**Backend:**
- Edit endpoints in `Program.cs` or create separate endpoint files
- Restart with `npm run backend:dev` or `dotnet run` (watch mode with `dotnet watch run`)
- Use `appsettings.json` for configuration

### Testing

**Frontend:**
```bash
# Write tests in *.spec.ts files
npm run frontend:test:watch     # Watch mode (recommended)
npm run frontend:test:single    # CI mode
```

**Backend:**
- Add xUnit test project when needed: `dotnet new xunit -n backend.Tests`
- Run: `npm run backend:test`

## Environment & Dependencies

**Frontend:**
- Node.js 18+ (via package.json engines, optional)
- pnpm 8+ (for workspace management)
- Angular 19.x, TypeScript 5.6, RxJS 7.8

**Backend:**
- .NET SDK 9.0+
- Entity Framework Core 9.0 (included)
- SQL Server (local or Azure)

**Run Version Check:**
```bash
node --version
dotnet --version
pnpm --version
```

## CORS Configuration

The backend CORS policy currently allows:
- `http://localhost:4200` (Angular dev server)
- `http://localhost:3000` (alternate frontend port)

To add production URLs, update the CORS policy in `Program.cs`:
```csharp
policy.WithOrigins("https://yourdomain.com", ...)
```

## Common Commands Reference

| Task | Command |
|------|---------|
| Install deps | `npm run install` |
| Frontend dev | `npm run frontend:dev` |
| Backend dev | `npm run backend:dev` |
| Frontend build | `npm run frontend:build` |
| Backend build | `npm run backend:build` |
| Frontend test | `npm run frontend:test:watch` |
| Backend test | `npm run backend:test` |
| Frontend lint | `npm run frontend:lint` |

## Multi-Agent Workflow

This project is configured for multi-agent development with specialized agents:

**Frontend Specialist** — Handles Angular components, routing, styling, HTTP integration
**Backend Specialist** — Handles .NET APIs, database, business logic, validation

See `.claude/agents/` for:
- `frontend-specialist.md` — Frontend agent specialization guidelines
- `backend-specialist.md` — Backend agent specialization guidelines
- `COORDINATION.md` — Team workflow and decision matrix
- `WORKFLOW.md` — Agent invocation patterns and examples

**Quick example:**
```
"Frontend Specialist: Add a UserListComponent that displays users from the backend"
"Backend Specialist: Create a GET /api/users endpoint that returns a list of users"
```

## Notes for Future Development

- **API Client Service**: Create a typed service (e.g., `services/api.service.ts`) to centralize HTTP calls
- **State Management**: Consider NgRx or Signals (Angular 19 feature) as complexity grows
- **Backend Organization**: As endpoints grow, separate into feature modules or organized folders
- **Database**: Entity Framework Core is configured; create DbContext and models when ready
- **Deployment**: Build artifacts are in `apps/frontend/dist` and `apps/backend/bin`
- **Environment-Specific Settings**: Use `appsettings.Development.json` for local overrides

