@ECHO OFF
SETLOCAL

echo ========================================
echo  GERANDO RELATORIO DE COBERTURA
echo ========================================
echo.

REM Instala ferramentas se necessario (ignora erro se ja tiver)
dotnet tool install --global coverlet.console 2>nul
dotnet tool install --global dotnet-reportgenerator-globaltool 2>nul

echo [1/4] Restaurando dependencias...
dotnet restore Ambev.DeveloperEvaluation.sln
if errorlevel 1 exit /b 1

echo [2/4] Compilando solucao...
dotnet build Ambev.DeveloperEvaluation.sln --configuration Release --no-restore
if errorlevel 1 exit /b 1

REM Limpa resultados anteriores
if exist "TestResults" rmdir /s /q "TestResults"

echo [3/4] Executando testes unitarios com cobertura...
dotnet test tests/Ambev.DeveloperEvaluation.Unit/Ambev.DeveloperEvaluation.Unit.csproj ^
    --no-restore --verbosity normal ^
    /p:CollectCoverage=true ^
    /p:CoverletOutputFormat=cobertura ^
    /p:CoverletOutput=../../TestResults/coverage.cobertura.xml ^
    /p:ExcludeByFile="**\Migrations\*.cs"

if errorlevel 1 (
    echo ERRO: Falha ao executar testes
    pause
    exit /b 1
)

echo [4/4] Gerando relatorio HTML...
reportgenerator ^
    -reports:"TestResults/coverage.cobertura.xml" ^
    -targetdir:"TestResults/CoverageReport" ^
    -reporttypes:"Html;Badges;TextSummary"

if errorlevel 1 (
    echo ERRO: Falha ao gerar relatorio
    pause
    exit /b 1
)

echo.
echo ========================================
echo  RELATORIO GERADO COM SUCESSO!
echo ========================================
echo.
echo Localizacao: TestResults\CoverageReport\index.html
echo.

if exist "TestResults\CoverageReport\Summary.txt" (
    echo --- RESUMO DA COBERTURA ---
    type "TestResults\CoverageReport\Summary.txt"
    echo.
)

REM Cria um atalho simples para o relatorio
echo Criando arquivo abrir-relatorio.bat na raiz...

echo @echo off> abrir-relatorio.bat
echo start "" "TestResults\CoverageReport\index.html">> abrir-relatorio.bat

echo.
echo Agora voce pode abrir o relatorio apenas dando dois cliques em: abrir-relatorio.bat
echo.

pause