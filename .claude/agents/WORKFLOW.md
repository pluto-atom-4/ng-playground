# Multi-Agent Workflow Usage Guide

## Quick Reference: Which Agent to Use?

### Frontend Task
```
"[Frontend Specialist] Add a new component that displays user profile"
```
→ Engages with `apps/frontend/src/app/` focused mindset

### Backend Task
```
"[Backend Specialist] Create a GET /api/users endpoint with filtering"
```
→ Engages with `apps/backend/Program.cs` focused mindset

### Architecture Decision
```
"[Plan Agent] Design how to structure state management across the application"
```
→ Reviews entire codebase, designs multi-layer approach

### Code Exploration
```
"[Explore Agent] Find all HTTP calls in the frontend and analyze patterns"
```
→ Rapidly searches and categorizes code patterns

## Agent Invocation Examples

### Example 1: Adding Backend Feature
**You say:**
> "Backend Specialist: Add a POST /api/users endpoint that creates a new user with name and email. Return the created user with an ID."

**Agent will:**
1. Review Program.cs for existing patterns
2. Check appsettings.json for configuration
3. Add endpoint with proper error handling
4. Document request/response format
5. Suggest Frontend Specialist next steps

### Example 2: Frontend Integration Task
**You say:**
> "Frontend Specialist: Create a UserService that calls the GET /api/users endpoint and a UserListComponent to display results with loading and error states."

**Agent will:**
1. Review app.config.ts for HTTP client setup
2. Create service in apps/frontend/src/app/services/
3. Create component in apps/frontend/src/app/pages/
4. Add proper typing and error handling
5. Suggest tests needed

### Example 3: Planning Complex Feature
**You say:**
> "Plan Agent: Design how we should implement user authentication with JWT tokens across the full stack."

**Agent will:**
1. Analyze current architecture
2. Design backend token generation logic
3. Design frontend token storage and refresh
4. Create step-by-step implementation plan
5. Suggest which specialist agents to use for each step

### Example 4: Code Analysis
**You say:**
> "Explore Agent: Find all instances where we're making HTTP requests and identify any CORS issues or error handling gaps."

**Agent will:**
1. Search frontend for HTTP calls
2. Review backend CORS configuration
3. Identify patterns and inconsistencies
4. Report findings and suggest improvements

## Workflow Patterns

### Pattern A: Feature Implementation (3 steps)
```
1. Plan Agent: Design the feature
   └─ Output: Architecture document, API contract

2. Backend Specialist: Implement API
   └─ Output: New endpoints, ready for testing

3. Frontend Specialist: Implement UI
   └─ Input: API contract from step 1
   └─ Output: Components calling backend
```

### Pattern B: Bug Investigation (2 steps)
```
1. Explore Agent: Find related code
   └─ Output: Files involved, context

2. Specialist Agent (Frontend or Backend): Fix issue
   └─ Input: Context from Explore Agent
   └─ Output: Fixed code, tests
```

### Pattern C: Optimization (3 steps)
```
1. Explore Agent: Analyze performance
   └─ Output: Bottlenecks identified

2. Relevant Specialist: Optimize code
   └─ Output: Improved implementation

3. Plan Agent: Review architecture changes
   └─ Output: Approval, broader implications
```

### Pattern D: Cross-Team Integration (3+ steps)
```
1. Backend Specialist: Create API endpoint
   └─ Output: Tested endpoint

2. Frontend Specialist: Integrate with UI
   └─ Output: Component consuming API

3. Both Specialists: Integration testing
   └─ Output: Verified end-to-end flow
```

## Communication Between Agents

While agents can't directly communicate, you can use this pattern:

**Step 1 - Backend prepares:**
```
"Backend Specialist: Create a POST /api/tasks endpoint that accepts
{ title: string, description: string } and returns { id: number, title, description, createdAt: datetime }"
```
[Agent creates endpoint]

**Step 2 - Frontend consumes:**
```
"Frontend Specialist: Create TaskService and CreateTaskComponent that calls POST /api/tasks
with the contract from the backend (reference: POST /api/tasks endpoint)"
```
[Agent reads backend code and implements matching frontend]

## Best Practices

### ✅ Do This
- Be specific about which specialist you're using
- Reference specific files when relevant
- Ask agents to verify both sides work together
- Use Explore Agent to understand patterns before changing them
- Document API contracts in COORDINATION.md when adding new endpoints

### ❌ Don't Do This
- Switch agents mid-task without clear handoff
- Assume one specialist knows the entire codebase (use Explore if needed)
- Make major architectural changes without Plan Agent review
- Forget to test integration after specialist work

## Scaling the Agent Team

### When You Have 2-3 Team Members
- Frontend Specialist owns `apps/frontend/`
- Backend Specialist owns `apps/backend/`
- Use Plan Agent for decisions affecting both

### When Feature Complexity Grows
- Create sub-specialists by feature area (e.g., "Auth Specialist", "Data Specialist")
- Use existing agents as templates
- Update COORDINATION.md with new team structure

### When Multiple People Use Claude Code
- Each person can reference this COORDINATION.md
- Agents will pull context from shared files
- Use `.claude/agents/` as source of truth for responsibilities

---

**Tip**: Save this file and COORDINATION.md. Reference them when starting new tasks to get the right agent engaged from the start.
