# Full-Stack NG Playground

A minimal full-stack monorepo with Angular 19 frontend and .NET 9 backend APIs.

## Quick Start

### 1. Install Dependencies

```bash
npm run install
```

### 2. Run in Development

**Terminal 1 - Frontend:**
```bash
npm run frontend:dev
```
Frontend runs on http://localhost:4200

**Terminal 2 - Backend:**
```bash
npm run backend:dev
```
Backend runs on http://localhost:5000

### 3. Test the Integration

Open http://localhost:4200 and click "Fetch Message" to call the backend health endpoint.

## Project Structure

- **apps/frontend/** — Angular 19 standalone components application
- **apps/backend/** — .NET 9 minimal APIs with CORS configured
- **pnpm-workspace.yaml** — Workspace configuration

## Key Commands

```bash
# Frontend
npm run frontend:dev         # Start dev server
npm run frontend:build       # Production build
npm run frontend:test:watch  # Run tests with watch
npm run frontend:lint        # Lint TypeScript

# Backend
npm run backend:dev          # Start API server
npm run backend:build        # Compile project
npm run backend:test         # Run tests
```

## Technology Stack

| Layer | Tech |
|-------|------|
| Frontend | Angular 19, TypeScript, SCSS |
| Backend | .NET 9, C#, Minimal APIs |
| Database | Azure SQL (configured) |
| Package Manager | pnpm |

## Features

✅ Angular standalone components
✅ Typed HTTP client integration
✅ CORS enabled between frontend/backend
✅ .NET minimal APIs
✅ Entity Framework Core setup
✅ Hot reload in development

## Development Tips

- **Angular**: Components are in `apps/frontend/src/app/`. Modify and save for instant reload.
- **.NET**: Endpoints are in `Program.cs`. Restart `npm run backend:dev` to see changes.
- **Database**: Update connection string in `appsettings.json` for Azure SQL or local SQL Server.
- **Testing**: Frontend tests with Jasmine/Karma, backend tests with xUnit (configure as needed).

## Documentation

See [CLAUDE.md](./CLAUDE.md) for detailed architecture and development guidelines.
