@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

REM ============================================
REM CLI para aplicar migrations no banco
REM Uso:
REM   cli-update-database
REM ============================================

REM Detecta a pasta onde o script esta
SET SCRIPT_DIR=%~dp0

REM Remove a barra invertida final, se existir
IF "%SCRIPT_DIR:~-1%"=="\" SET SCRIPT_DIR=%SCRIPT_DIR:~0,-1%

REM Define raiz da solucao como a pasta do script
SET SOLUTION_ROOT=%SCRIPT_DIR%

REM Caminhos dos projetos (relativos à raiz)
SET ORM_PROJECT=%SOLUTION_ROOT%\src\Ambev.DeveloperEvaluation.ORM\Ambev.DeveloperEvaluation.ORM.csproj
SET STARTUP_PROJECT=%SOLUTION_ROOT%\src\Ambev.DeveloperEvaluation.WebApi\Ambev.DeveloperEvaluation.WebApi.csproj
SET CONTEXT_NAME=DefaultContext

IF NOT EXIST "%ORM_PROJECT%" (
    echo ORM project not found at "%ORM_PROJECT%"
    EXIT /B 1
)

IF NOT EXIST "%STARTUP_PROJECT%" (
    echo Startup project not found at "%STARTUP_PROJECT%"
    EXIT /B 1
)

echo Updating database...
echo Using ORM project: "%ORM_PROJECT%"
echo Using Startup project: "%STARTUP_PROJECT%"

dotnet ef database update ^
    --project "%ORM_PROJECT%" ^
    --startup-project "%STARTUP_PROJECT%" ^
    --context %CONTEXT_NAME%

IF %ERRORLEVEL% NEQ 0 (
    echo Failed to update database.
    EXIT /B %ERRORLEVEL%
)

echo Database updated successfully.
ENDLOCAL