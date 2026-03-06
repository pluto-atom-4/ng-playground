# Multi-Agent Workflow Coordination

## Team Structure

### 🎨 Frontend Specialist Agent
- **Responsibility**: Angular frontend (apps/frontend/)
- **Skills**: TypeScript, Angular 19, SCSS, RxJS, HTTP client
- **When to engage**: Component development, styling, routing, frontend testing

### ⚙️ Backend Specialist Agent
- **Responsibility**: .NET 9 API backend (apps/backend/)
- **Skills**: C#, .NET 9, EF Core, REST APIs, Azure SQL
- **When to engage**: Endpoint development, data models, database integration, backend testing

### 🏗️ Plan Agent
- **Responsibility**: Architecture and multi-layer decisions
- **When to engage**: Major features, cross-team decisions, system design

### 🔍 Explore Agent
- **Responsibility**: Codebase analysis and understanding
- **When to engage**: Finding files, understanding existing patterns, performance analysis

## Communication Protocol

### Task Handoff Between Frontend → Backend
1. **Frontend** identifies needed API endpoint
2. **Frontend** documents request/response structure
3. **Backend** implements endpoint with proper CORS headers
4. **Frontend** integrates using typed HTTP client
5. **Frontend** tests integration with backend running locally

### Task Handoff Between Backend → Frontend
1. **Backend** documents new API endpoint
2. **Backend** provides example responses
3. **Frontend** creates component/service to consume endpoint
4. **Frontend** handles loading states and errors
5. **Both** verify integration works end-to-end

## Common Integration Points

### API Contract
```
Endpoint: GET /api/health
Frontend Path: apps/frontend/src/app/pages/home/home.component.ts (line: fetchMessage())
Backend Path: apps/backend/Program.cs (line: app.MapGet("/api/health", ...))
Response: { message: string }
```

### Development Environment
- Frontend: http://localhost:4200
- Backend: http://localhost:5000
- Both must run simultaneously
- CORS enabled between them

## Workflow Example: Add New Feature

### Step 1: Planning (Plan Agent)
- Define feature requirements
- Decide API structure
- Design data models (if needed)

### Step 2: Backend Development (Backend Specialist)
- Create/update API endpoints in Program.cs
- Add data models and EF Core migrations
- Test endpoints with tools like curl or Postman

### Step 3: Frontend Development (Frontend Specialist)
- Create components to consume endpoints
- Implement error handling
- Add loading states
- Write tests

### Step 4: Integration Testing
- Run both servers
- Test full workflow
- Verify error handling

### Step 5: Deployment (Plan Agent)
- Build frontend: `npm run frontend:build`
- Build backend: `npm run backend:build`
- Document breaking changes

## Decision Matrix

| Decision Type | Lead Agent | Input From |
|---------------|-----------|-----------|
| API endpoint design | Backend Specialist | Frontend needs |
| Frontend UI/UX | Frontend Specialist | Design requirements |
| Database schema | Backend Specialist | Frontend data needs |
| Project structure | Plan Agent | Both specialists |
| Performance optimization | Explore Agent | Both specialists |
| Security/validation | Backend Specialist | Plan agent guidance |

## Documentation Locations

- **Project Overview**: `/CLAUDE.md`
- **Quick Start**: `/README.md`
- **Frontend Guidelines**: `apps/frontend/` (see README patterns)
- **Backend Guidelines**: `apps/backend/` (endpoints in Program.cs)
- **Agent Specializations**: `.claude/agents/` (this directory)

## Running Multi-Agent Tasks

### Option A: Sequential (Default)
1. Use Frontend Specialist for feature A
2. Use Backend Specialist for feature B
3. Coordinate integration

### Option B: Parallel
1. Frontend Specialist works on component UI
2. Backend Specialist works on API logic simultaneously
3. Both reference COORDINATION.md for expected contracts

### Option C: Iterative
1. Backend creates basic endpoint
2. Frontend creates integration
3. Backend refines endpoint based on feedback
4. Frontend updates error handling/UI

## Escalation & Collaboration

**When to bring in Plan Agent:**
- Major architectural changes
- Cross-team dependency questions
- Technology upgrade decisions
- Performance bottlenecks

**When to bring in Explore Agent:**
- Finding related code
- Understanding existing patterns
- Codebase metrics or analysis
- Refactoring scope assessment

## Context Sharing

Each agent has access to:
- This COORDINATION.md file
- Project CLAUDE.md
- Respective specialist guidelines
- Memory files for persistent knowledge

---

**Last Updated**: 2026-03-05
