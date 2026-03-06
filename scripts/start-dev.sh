#!/bin/bash

# Start development server(s) based on command-line arguments
# Usage: ./start-dev.sh [OPTION]
# Options:
#   --front-end, -f     Start only the frontend dev server
#   --back-end, -b      Start only the backend dev server
#   --full-stack, -a    Start both frontend and backend (default)
#   --help, -h          Show this help message

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

# Default to full-stack if no argument provided
MODE="full-stack"

# Parse command-line arguments
while [[ $# -gt 0 ]]; do
  case "$1" in
    --front-end|-f)
      MODE="frontend"
      shift
      ;;
    --back-end|-b)
      MODE="backend"
      shift
      ;;
    --full-stack|-a)
      MODE="full-stack"
      shift
      ;;
    --help|-h)
      echo "Usage: $0 [OPTION]"
      echo ""
      echo "Start development server(s) for ng-playground"
      echo ""
      echo "Options:"
      echo "  --front-end, -f     Start only the frontend dev server"
      echo "  --back-end, -b      Start only the backend dev server"
      echo "  --full-stack, -a    Start both frontend and backend (default)"
      echo "  --help, -h          Show this help message"
      exit 0
      ;;
    *)
      echo "Error: Unknown option '$1'"
      echo "Use --help or -h for usage information"
      exit 1
      ;;
  esac
done

cd "$PROJECT_ROOT"

# Start servers based on mode
case "$MODE" in
  frontend)
    echo "Starting frontend dev server..."
    npm run frontend:dev
    ;;
  backend)
    echo "Starting backend dev server..."
    npm run backend:dev
    ;;
  full-stack)
    echo "Starting both frontend and backend dev servers..."
    # Run both in parallel
    npm run frontend:dev &
    FRONTEND_PID=$!
    
    npm run backend:dev &
    BACKEND_PID=$!
    
    # Wait for both processes (Ctrl+C will terminate both)
    trap "kill $FRONTEND_PID $BACKEND_PID 2>/dev/null || true" EXIT INT TERM
    
    echo ""
    echo "=========================================="
    echo "Frontend: http://localhost:4200"
    echo "Backend:  http://localhost:5000"
    echo "=========================================="
    echo "Press Ctrl+C to stop all servers"
    echo "=========================================="
    echo ""
    
    wait
    ;;
esac
