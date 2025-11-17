#!/usr/bin/env bash
set -e

# ============================================
# CLI para aplicar migrations no banco
# Uso:
#   ./cli-update-database.sh
# ============================================

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
SOLUTION_ROOT="$SCRIPT_DIR"

ORM_PROJECT="$SOLUTION_ROOT/src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj"
STARTUP_PROJECT="$SOLUTION_ROOT/src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"
CONTEXT_NAME="DefaultContext"

if [ ! -f "$ORM_PROJECT" ]; then
  echo "ORM project not found at '$ORM_PROJECT'"
  exit 1
fi

if [ ! -f "$STARTUP_PROJECT" ]; then
  echo "Startup project not found at '$STARTUP_PROJECT'"
  exit 1
fi

echo "Updating database..."
echo "Using ORM project: $ORM_PROJECT"
echo "Using Startup project: $STARTUP_PROJECT"

dotnet ef database update \
  --project "$ORM_PROJECT" \
  --startup-project "$STARTUP_PROJECT" \
  --context "$CONTEXT_NAME"

echo "Database updated successfully."