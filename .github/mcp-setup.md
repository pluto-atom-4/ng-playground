# MCP Server Configuration for ng-playground

This guide sets up MCP servers to enhance development workflows for the ng-playground project.

## Prerequisites

- Node.js 18+ and npm/pnpm
- For Playwright: Modern browser installed (Chrome, Firefox, or WebKit)

## Installing MCP Servers

### 1. Playwright MCP Server

Playwright enables end-to-end (E2E) testing and browser automation for your Angular frontend.

**Installation:**
```bash
npm install -g @modelcontextprotocol/server-playwright
```

**Use cases:**
- Test Angular components in a real browser
- Automate user interactions (clicks, form fills, navigation)
- Verify frontend behavior with backend APIs
- Screenshot and visual regression testing

**Example workflow:**
```bash
# Start both frontend and backend
npm run frontend:dev  # Terminal 1
npm run backend:dev   # Terminal 2

# Use Playwright MCP to test integration
# (Playwright can navigate to http://localhost:4200 and interact with your Angular app)
```

### 2. SQLite MCP Server

SQLite MCP allows querying local databases during development, useful for:
- Debugging data issues
- Verifying Entity Framework migrations
- Testing database queries before deploying to Azure SQL

**Installation:**
```bash
npm install -g @modelcontextprotocol/server-sqlite
```

**Use cases:**
- Query local SQLite database for testing
- Inspect schema and data during development
- Validate migrations before applying to production

**Note:** Your backend uses SQL Server / Azure SQL, not SQLite. You can use a local SQLite database for local testing alongside your SQL Server setup.

## Configuration in Your IDE/Editor

### For Claude (claude.ai/code)

Add to your Claude MCP configuration (if using Claude IDE integration):
```json
{
  "mcpServers": {
    "playwright": {
      "command": "npx",
      "args": ["@modelcontextprotocol/server-playwright"]
    },
    "sqlite": {
      "command": "npx",
      "args": ["@modelcontextprotocol/server-sqlite"]
    }
  }
}
```

### For VS Code / Other Editors

If your editor supports MCP server configuration, reference the server installation paths above.

## Common MCP Server Tasks

### Testing with Playwright

```typescript
// Pseudo-code example - use with Playwright MCP
// Navigate to your Angular app
goto('http://localhost:4200')

// Interact with components
click('button:has-text("Fetch Message")')

// Verify API calls worked
waitForSelector('text=Backend is healthy')

// Take screenshot
screenshot('healthcheck.png')
```

### Querying Data with SQLite

```sql
-- Query local SQLite database
SELECT * FROM users;
SELECT * FROM aircraft WHERE status = 'active';
```

## Benefits

- **Playwright**: Automated browser testing without manual clicking; verify frontend-backend integration
- **SQLite**: Quick data inspection and schema validation without SQL Server Management Studio

## Troubleshooting

**Playwright not finding browser:**
```bash
npx playwright install
```

**SQLite connection issues:**
Ensure your SQLite database path is correctly specified in the server configuration.

## Next Steps

1. Install the MCP servers via npm
2. Configure them in your IDE/editor
3. Use them alongside `npm run frontend:dev` and `npm run backend:dev` during development
4. Reference `.github/copilot-instructions.md` for project-specific commands

For more details on the project structure, see [copilot-instructions.md](./copilot-instructions.md).
