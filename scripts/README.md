# Development Scripts

This directory contains utility scripts for managing the ng-playground development environment.

## start-dev.sh

Shell script to start development servers for the full-stack application.

### Features

- Start **frontend only** (Angular dev server on `localhost:4200`)
- Start **backend only** (.NET dev server on `localhost:5000`)
- Start **both servers simultaneously** (full-stack mode)
- Run both servers in parallel with proper cleanup on exit

### Usage

```bash
./scripts/start-dev.sh [OPTION]
```

### Options

| Option | Short | Description |
|--------|-------|-------------|
| `--front-end` | `-f` | Start only the Angular frontend dev server |
| `--back-end` | `-b` | Start only the .NET backend dev server |
| `--full-stack` | `-a` | Start both frontend and backend (default if no option provided) |
| `--help` | `-h` | Display help message |

### Examples

```bash
# Start both frontend and backend (default)
./scripts/start-dev.sh

# Start frontend only
./scripts/start-dev.sh --front-end
./scripts/start-dev.sh -f

# Start backend only
./scripts/start-dev.sh --back-end
./scripts/start-dev.sh -b

# Start full-stack explicitly
./scripts/start-dev.sh --full-stack
./scripts/start-dev.sh -a

# Show help
./scripts/start-dev.sh --help
./scripts/start-dev.sh -h
```

### Server Endpoints

When running in full-stack mode, both servers are accessible at:

- **Frontend**: http://localhost:4200
- **Backend**: http://localhost:5000

Press `Ctrl+C` to stop all running servers.

### Requirements

- Node.js and npm/pnpm installed
- .NET SDK 9.0+ installed
- All dependencies installed (`npm run install`)

### How It Works

- **Single server mode**: Runs the specified server in the foreground
- **Full-stack mode**: Launches both servers as background processes, displays a summary, and waits for user termination
- **Cleanup**: Properly terminates all child processes when the script is interrupted

