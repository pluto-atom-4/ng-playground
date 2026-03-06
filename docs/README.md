# Documentation

Welcome to the ng-playground documentation. This directory contains guides and references for setting up, configuring, and maintaining the full-stack application.

## Quick Links

### Setup & Configuration
- **[Azure SQL Database Setup](./AZURE_SQL_SETUP.md)** — Complete guide for configuring the backend to connect to Azure SQL Database
  - Creating Azure resources via Azure CLI
  - Connection string configuration
  - Entity Framework Core migrations
  - Troubleshooting common issues
  - Security best practices

### Main Project Documentation
- **[CLAUDE.md](../CLAUDE.md)** — Project architecture, repository structure, development workflow, and commands
- **[README.md](../README.md)** — Getting started guide (root level)

### Architecture Guides (Future)
- *Database Design* (to be added)
- *API Endpoint Reference* (to be added)
- *Frontend Component Guide* (to be added)
- *Authentication & Authorization* (to be added)
- *Deployment Guide* (to be added)

---

## Document Organization

```
docs/
├── README.md                    # This file - documentation index
└── AZURE_SQL_SETUP.md          # Azure SQL configuration guide
```

---

## Getting Started

### First Time Setup
1. Read **[CLAUDE.md](../CLAUDE.md)** for project overview
2. Follow **[README.md](../README.md)** for installation
3. For database setup, follow **[AZURE_SQL_SETUP.md](./AZURE_SQL_SETUP.md)**

### Quick Command Reference
```bash
# Install dependencies
npm run install

# Start development servers
npm run frontend:dev    # Angular (http://localhost:4200)
npm run backend:dev     # .NET API (http://localhost:5000)

# Build for production
npm run frontend:build
npm run backend:build
```

---

## Troubleshooting

### Backend Connection Issues?
→ See [Azure SQL Setup - Troubleshooting](./AZURE_SQL_SETUP.md#troubleshooting)

### Frontend Not Building?
→ Check [CLAUDE.md - Development Workflow](../CLAUDE.md#development-workflow)

### Need help?
→ Check the relevant guide's FAQ section or open an issue on GitHub

---

## Contributing

When adding new documentation:
1. Create a new `.md` file in the `docs/` directory
2. Update this `README.md` with a link to the new guide
3. Use clear headings and code examples
4. Include a "Document Version" section with dates and versions
5. Commit with message: `docs: Add [topic] guide`

---

## Version Info

- **Project**: ng-playground
- **Frontend**: Angular 19
- **Backend**: .NET 9
- **Database**: Azure SQL (optional, configuration documented)
- **Docs Last Updated**: March 6, 2026
