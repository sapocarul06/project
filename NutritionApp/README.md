# NutrițieApp - Aplicație de Gestionare a Planurilor Alimentare

## Descriere
Aplicație Console în C# (.NET Framework 4.7.2) pentru gestionarea planurilor alimentare cu sugestii bazate pe setări prestabilite.

## Funcționalități Implementate

### 1. Calcul Necesar Caloric pentru Deficit
- Utilizează formula Mifflin-St Jeor pentru BMR
- Calculează TDEE în funcție de nivelul de activitate
- Determină necesarul caloric pentru deficit (pierdere în greutate)
- Calculează macronutrienții recomandați

**Intrări:**
- Înălțime (cm)
- Greutate (kg)
- Vârstă (ani)
- Sex (m/f)
- Activitate fizică (sedentar/moderat/intens)

**Ieșiri:**
- Necessar caloric pentru deficit
- Distribuție macronutrienți

### 2. Import Date de la Provideri Multipli
- Import din Kaggle (dataset-uri nutriționale)
- Import din USDA FoodData Central API
- Script pentru actualizare zilnică automată

### 3. Script Actualizare Zilnică (Cron)
- Rulează în fiecare zi pentru actualizarea datelor
- Configurare cron: `0 2 * * * /path/to/NutritionApp.exe --daily-update`

### 4. Identificare Anunțuri Duplicate pe Baza Imaginilor
- Generare hash perceptual pentru imagini
- Comparare imagini folosind distanța Hamming
- Detectare duplicate și similare

### 5. Studiu Comparativ Stocare Imagini
- FileSystem vs Database (BLOB) vs Cloud Storage
- Analiză avantaje/dezavantaje
- Recomandări pentru diferite scenarii

### 6. Listare Date Aplicație
- Vizualizare alimente din baza de date
- Filtrare și căutare
- Statistici summary

### 7. Setare Cron pentru Emailuri Promoționale
- Serviciu de trimitere emailuri
- Programare trimiteri periodice
- Conținut personalizabil

### 8. Notificări cu Fire de Execuție Paralele
- Procesare asincronă a notificărilor
- Thread pool pentru performanță
- Monitorizare anunțuri noi în timp real

### 9. Studiu Comparativ Local vs Server vs Serverless
- Analiză performanță stocare date
- Comparare costuri
- Recomandări arhitecturale

### 10. Integrare Serviciu Asincron
- Operații I/O asincrone
- Task-based async pattern
- Non-blocking operations

### 11. Generare Rapoarte PDF/Excel
- Rapoarte HTML convertibile la PDF
- Export Excel (CSV format)
- Rapoarte statistice detaliate

### 12. Statistici Consumatoare de Timp
- Benchmarking operațiuni
- Profilare performanță
- Analiză statistică avansată

## Structura Proiectului

```
NutritionApp/
├── Models/
│   ├── UserProfile.cs       # Profil utilizator
│   ├── FoodItem.cs          # Model aliment
│   └── MealPlan.cs          # Plan alimentar
├── Services/
│   ├── CalorieCalculator.cs      # Calcul caloric
│   ├── DataImportService.cs      # Import date
│   ├── ImageComparisonService.cs # Comparare imagini
│   ├── NotificationService.cs    # Notificări
│   ├── EmailService.cs           # Emailuri
│   ├── ReportGenerator.cs        # Rapoarte
│   └── StorageService.cs         # Stocare
├── Repositories/
│   └── DataRepository.cs    # Acces date
├── Utils/
│   ├── ImageHash.cs              # Hash imagini
│   └── StatisticsCalculator.cs   # Statistici
├── Scripts/
│   └── daily-update.sh      # Script actualizare
├── Docs/
│   └── storage-study.md     # Studii comparative
├── Data/                    # Date importate
├── Reports/                 # Rapoarte generate
├── Program.cs               # Entry point
└── NutritionApp.csproj      # Proiect .NET
```

## Cerințe Sistem

- .NET Framework 4.7.2 sau superior
- Windows (pentru cron, folosiți Task Scheduler)
- Linux/Mac (pentru cron jobs native)

## Compilare

```bash
# Cu MSBuild (Windows)
msbuild NutritionApp.csproj /p:Configuration=Release

# Sau cu Visual Studio
Deschideți NutritionApp.csproj în VS și build
```

## Utilizare

Rulați aplicația:
```bash
NutritionApp.exe
```

Urmați meniul interactiv pentru a accesa funcționalitățile.

## Configurare Cron (Linux/Mac)

Pentru actualizare zilnică la ora 2:00:
```bash
crontab -e
# Adăugați linia:
0 2 * * * /usr/bin/mono /path/to/NutritionApp.exe --daily-update
```

Pentru emailuri promoționale săptămânale (Luni 9:00):
```bash
0 9 * * 1 /usr/bin/mono /path/to/NutritionApp.exe --send-promo-emails
```

## Configurare Task Scheduler (Windows)

1. Deschideți Task Scheduler
2. Create Basic Task
3. Set trigger: Daily at 2:00 AM
4. Set action: Start a program
5. Program: `C:\path\to\NutritionApp.exe`
6. Arguments: `--daily-update`

## Formule Utilizate

### Formula Mifflin-St Jeor pentru BMR
- **Bărbați:** BMR = 10×greutate(kg) + 6.25×înălțime(cm) - 5×vârstă(ani) + 5
- **Femei:** BMR = 10×greutate(kg) + 6.25×înălțime(cm) - 5×vârstă(ani) - 161

### TDEE (Total Daily Energy Expenditure)
- Sedentar: BMR × 1.2
- Moderat: BMR × 1.55
- Intens: BMR × 1.725

### Deficit Caloric
- Deficit standard: TDEE - 500 kcal (pentru ~0.5 kg/săptămână)
- Minim sigur: 1200 kcal (femei), 1500 kcal (bărbați)

## License

Acest proiect este creat în scop educațional.
