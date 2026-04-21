#!/bin/bash
# Script pentru actualizare zilnică a datelor NutritionApp
# Configurare cron: 0 2 * * * /path/to/daily-update.sh

echo "=========================================="
echo "NutritionApp - Actualizare Zilnică"
echo "Data: $(date)"
echo "=========================================="

# Set directory
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
APP_DIR="$SCRIPT_DIR/../NutritionApp"
LOG_FILE="$SCRIPT_DIR/daily-update.log"

# Function to log messages
log() {
    echo "[$(date '+%Y-%m-%d %H:%M:%S')] $1" | tee -a "$LOG_FILE"
}

log "Începe actualizarea zilnică..."

# Verifică dacă .NET/Mono este instalat
if command -v mono &> /dev/null; then
    RUNTIME="mono"
elif command -v dotnet &> /dev/null; then
    RUNTIME="dotnet"
else
    log "EROARE: Nu s-a găsit .NET sau Mono instalat"
    exit 1
fi

log "Runtime detectat: $RUNTIME"

# Rulează actualizarea
if [ -f "$APP_DIR/NutritionApp.exe" ]; then
    $RUNTIME "$APP_DIR/NutritionApp.exe" --daily-update >> "$LOG_FILE" 2>&1
    EXIT_CODE=$?
    
    if [ $EXIT_CODE -eq 0 ]; then
        log "Actualizare completă cu succes"
    else
        log "EROARE: Actualizare eșuată (exit code: $EXIT_CODE)"
    fi
else
    log "EROARE: NutritonApp.exe nu a fost găsit"
    exit 1
fi

# Curăță log-urile vechi (păstrează ultimele 30 de zile)
find "$SCRIPT_DIR" -name "*.log" -mtime +30 -delete 2>/dev/null

log "Script actualizare încheiat"
echo "=========================================="
