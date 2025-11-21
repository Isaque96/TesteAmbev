#!/bin/bash

echo "========================================"
echo " GERANDO RELATORIO DE COBERTURA"
echo "========================================"
echo ""

# Instala ferramentas se necessario (ignora erro se ja tiver)
dotnet tool install --global coverlet.console 2>/dev/null
dotnet tool install --global dotnet-reportgenerator-globaltool 2>/dev/null

echo "[1/4] Restaurando dependencias..."
dotnet restore Ambev.DeveloperEvaluation.sln || exit 1

echo "[2/4] Compilando solucao..."
dotnet build Ambev.DeveloperEvaluation.sln --configuration Release --no-restore || exit 1

# Limpa resultados anteriores
rm -rf TestResults

echo "[3/4] Executando testes unitarios com cobertura..."
dotnet test tests/Ambev.DeveloperEvaluation.Unit/Ambev.DeveloperEvaluation.Unit.csproj \
    --no-restore --verbosity normal \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=../../TestResults/coverage.cobertura.xml || exit 1

echo "[4/4] Gerando relatorio HTML..."
reportgenerator \
    -reports:"TestResults/coverage.cobertura.xml" \
    -targetdir:"TestResults/CoverageReport" \
    -reporttypes:"Html;Badges;TextSummary" || exit 1

echo ""
echo "========================================"
echo " RELATORIO GERADO COM SUCESSO!"
echo "========================================"
echo ""
echo "Localizacao: TestResults/CoverageReport/index.html"
echo ""

if [ -f "TestResults/CoverageReport/Summary.txt" ]; then
    echo "--- RESUMO DA COBERTURA ---"
    cat "TestResults/CoverageReport/Summary.txt"
    echo ""
fi

echo "Criando atalho abrir-relatorio.sh na raiz..."
echo '#!/bin/bash' > abrir-relatorio.sh
echo 'xdg-open "TestResults/CoverageReport/index.html" 2>/dev/null || open "TestResults/CoverageReport/index.html"' >> abrir-relatorio.sh
chmod +x abrir-relatorio.sh

echo ""
echo "Agora voce pode abrir o relatorio com: ./abrir-relatorio.sh"