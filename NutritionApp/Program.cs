using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NutritionApp.Models;
using NutritionApp.Services;
using NutritionApp.Utils;
using NutritionApp.Repositories;

namespace NutritionApp
{
    /// <summary>
    /// Aplicație Console pentru gestionarea planurilor alimentare
    /// Tema: Proiectarea și implementarea unei aplicații de gestionare a unui plan alimentar 
    /// cu includerea sugestiilor în baza unor setări prestabilite
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            ShowHeader();
            
            var repository = new DataRepository();
            var importService = new DataImportService();
            var calorieCalculator = new CalorieCalculator();
            var reportGenerator = new ReportGenerator();
            var storageService = new StorageService();
            var notificationService = new NotificationService();
            var imageComparisonService = new ImageComparisonService();
            var statisticsCalculator = new StatisticsCalculator();
            
            bool continueRunning = true;
            
            while (continueRunning)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        CalculateCalorieDeficit(calorieCalculator, repository);
                        break;
                    case "2":
                        ImportData(importService, repository);
                        break;
                    case "3":
                        RunDailyUpdateScript(importService);
                        break;
                    case "4":
                        DetectDuplicateImages(imageComparisonService);
                        break;
                    case "5":
                        storageService.PrintStorageComparisonStudy();
                        break;
                    case "6":
                        ListData(repository);
                        break;
                    case "7":
                        // Email service ar necesita configurare SMTP reală
                        Console.WriteLine("[INFO] Serviciul de email necesită configurare SMTP.");
                        break;
                    case "8":
                        RunNotificationDemo(notificationService, repository);
                        break;
                    case "9":
                        statisticsCalculator.RunPerformanceBenchmark();
                        break;
                    case "10":
                        RunAsyncServiceDemo();
                        break;
                    case "11":
                        GenerateReports(reportGenerator, repository);
                        break;
                    case "12":
                        RunStatisticsDemo(statisticsCalculator);
                        break;
                    case "0":
                        continueRunning = false;
                        Console.WriteLine("La revedere!");
                        break;
                    default:
                        Console.WriteLine("Opțiune invalidă. Încercați din nou.");
                        break;
                }
                
                if (continueRunning)
                {
                    Console.WriteLine("\nApăsați orice tastă pentru a continua...");
                    Console.ReadKey();
                    Console.Clear();
                    ShowHeader();
                }
            }
        }
        
        static void ShowHeader()
        {
            Console.WriteLine(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                         NUTRITION APP - PLAN ALIMENTAR                       ║
║     Proiectarea și implementarea unei aplicații de gestionare a unui         ║
║              plan alimentar cu sugestii bazate pe setări prestabilite        ║
╚══════════════════════════════════════════════════════════════════════════════╝
");
        }
        
        static void ShowMainMenu()
        {
            Console.WriteLine(@"
┌─────────────────────────────────────────────────────────────────────────────┐
│                              MENIU PRINCIPAL                                 │
├─────────────────────────────────────────────────────────────────────────────┤
│  1.  Calculează necesarul caloric pentru deficit                             │
│  2.  Importă date de la provideri (Kaggle, USDA)                             │
│  3.  Rulează script actualizare zilnică (cron)                               │
│  4.  Identifică anunțuri duplicate pe baza imaginilor                        │
│  5.  Studiu comparativ stocare imagini (Server vs DB vs Cloud)               │
│  6.  Listează datele din aplicație                                           │
│  7.  Setare cron pentru emailuri promoționale                                │
│  8.  Demo notificări cu fire paralele                                        │
│  9.  Studiu comparativ Local vs Server vs Serverless                         │
│  10. Integrare serviciu asincron                                             │
│  11. Generare rapoarte PDF / Excel                                           │
│  12. Statistici consumatoare de timp                                         │
│                                                                              │
│  0.  Ieșire                                                                  │
└─────────────────────────────────────────────────────────────────────────────┘
Alege o opțiune: ");
        }
        
        static void CalculateCalorieDeficit(CalorieCalculator calculator, DataRepository repository)
        {
            Console.WriteLine("\n=== CALCUL NECESAR CALORIC PENTRU DEFICIT ===\n");
            
            try
            {
                Console.Write("Înălțime (cm): ");
                double height = double.Parse(Console.ReadLine());
                
                Console.Write("Greutate (kg): ");
                double weight = double.Parse(Console.ReadLine());
                
                Console.Write("Vârstă (ani): ");
                int age = int.Parse(Console.ReadLine());
                
                Console.Write("Sex (m/f): ");
                char sex = char.ToLower(Console.ReadLine()[0]);
                
                Console.WriteLine("\nNivel activitate fizică:");
                Console.WriteLine("  1. Sedentar (fără exerciții)");
                Console.WriteLine("  2. Moderat (3-5 zile/săptămână)");
                Console.WriteLine("  3. Intens (6-7 zile/săptămână)");
                Console.Write("Alege: ");
                int activityChoice = int.Parse(Console.ReadLine());
                
                UserProfile.ActivityLevel activityLevel = activityChoice switch
                {
                    2 => UserProfile.ActivityLevel.Moderat,
                    3 => UserProfile.ActivityLevel.Intens,
                    _ => UserProfile.ActivityLevel.Sedentar
                };
                
                var profile = new UserProfile
                {
                    HeightCm = height,
                    WeightKg = weight,
                    Age = age,
                    Sex = sex,
                    ActivityLevel = activityLevel
                };
                
                if (!profile.Validate())
                {
                    Console.WriteLine("Date invalide! Verificați valorile introduse.");
                    return;
                }
                
                repository.AddUser(profile);
                
                double bmr = CalorieCalculator.CalculateBMR(profile);
                double tdee = CalorieCalculator.CalculateTDEE(profile);
                int deficitCalories = CalorieCalculator.CalculateCalorieDeficit(profile);
                var macros = CalorieCalculator.CalculateMacros(profile, deficitCalories);
                
                Console.WriteLine($@"
┌─────────────────────────────────────────────────────────────────────────────┐
│                           REZULTATE CALCUL                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│  Rata Metabolică Bazală (BMR):     {bmr:F0} kcal/zi                          │
│  Necesar Caloric Total (TDEE):     {tdee:F0} kcal/zi                          │
│                                                                              │
│  ➤ NECESAR PENTRU DEFICIT:           {deficitCalories} kcal/zi                  │
│                                                                              │
│  Distribuție Macronutrienți:                                                 │
│    • Proteine:     {macros.ProteinGrams}g ({macros.ProteinGrams * 4} kcal)                      │
│    • Carbohidrați: {macros.CarbsGrams}g ({macros.CarbsGrams * 4} kcal)                      │
│    • Grăsimi:      {macros.FatGrams}g ({macros.FatGrams * 9} kcal)                       │
│                                                                              │
│  Notă: Acest deficit va duce la o pierdere de ~0.5 kg/săptămână             │
└─────────────────────────────────────────────────────────────────────────────┘
");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare: {ex.Message}");
            }
        }
        
        static void ImportData(DataImportService importService, DataRepository repository)
        {
            Console.WriteLine("\n=== IMPORT DATE DE LA PROVIDERI ===\n");
            
            Console.WriteLine("1. Import din Kaggle");
            Console.WriteLine("2. Import din USDA");
            Console.Write("Alege provider: ");
            string choice = Console.ReadLine();
            
            List<FoodItem> foodItems = choice switch
            {
                "1" => importService.ImportFromKaggle(""),
                "2" => importService.ImportFromUSDA("demo-key", "healthy foods"),
                _ => importService.ImportFromKaggle("")
            };
            
            foreach (var item in foodItems)
            {
                repository.AddFoodItem(item);
            }
            
            Console.WriteLine($"\n{foodItems.Count} alimente au fost importate și salvate.");
        }
        
        static void RunDailyUpdateScript(DataImportService importService)
        {
            Console.WriteLine("\n=== SCRIPT ACTUALIZARE ZILNICĂ ===\n");
            Console.WriteLine("Acest script ar fi configurat în cron să ruleze zilnic.");
            Console.WriteLine("Exemplu crontab: 0 2 * * * /path/to/NutritionApp.exe --daily-update\n");
            importService.DailyUpdateScript();
        }
        
        static void DetectDuplicateImages(ImageComparisonService comparisonService)
        {
            Console.WriteLine("\n=== DETECTARE IMAGINI DUPLICATE ===\n");
            comparisonService.RunDuplicateDetectionScript();
        }
        
        static void ListData(DataRepository repository)
        {
            Console.WriteLine("\n=== LISTARE DATE ===\n");
            
            var foodItems = repository.GetAllFoodItems();
            
            if (foodItems.Count == 0)
            {
                Console.WriteLine("Nu există date în baza de date. Importați mai întâi date.");
                return;
            }
            
            Console.WriteLine($"Total alimente: {foodItems.Count}\n");
            Console.WriteLine("┌──────┬──────────────────────┬────────────┬──────────┐");
            Console.WriteLine("│ ID   │ Nume                 │ Categorie  │ Calorii  │");
            Console.WriteLine("├──────┼──────────────────────┼────────────┼──────────┤");
            
            foreach (var item in foodItems)
            {
                Console.WriteLine($"│ {item.Id,-4} │ {item.Name,-20} │ {item.Category,-10} │ {item.CaloriesPer100g,-8} │");
            }
            
            Console.WriteLine("└──────┴──────────────────────┴────────────┴──────────┘");
        }
        
        static void RunNotificationDemo(NotificationService notificationService, DataRepository repository)
        {
            Console.WriteLine("\n=== DEMO NOTIFICĂRI CU FIRE PARALELE ===\n");
            
            notificationService.Start();
            
            var foodItems = repository.GetAllFoodItems();
            if (foodItems.Count == 0)
            {
                // Adăugăm date mock
                foodItems = new List<FoodItem>
                {
                    new FoodItem { Id = 1, Name = "Piept de pui", ProteinPer100g = 31, CarbsPer100g = 0 },
                    new FoodItem { Id = 2, Name = "Broccoli", ProteinPer100g = 2.8, CarbsPer100g = 7 },
                    new FoodItem { Id = 3, Name = "Orez brun", ProteinPer100g = 2.7, CarbsPer100g = 28 }
                };
            }
            
            Task.Run(async () =>
            {
                await notificationService.MonitorNewAnnouncementsAsync(foodItems);
            }).Wait();
            
            System.Threading.Thread.Sleep(2000); // Așteptăm procesarea
            notificationService.Stop();
        }
        
        static async Task RunAsyncServiceDemo()
        {
            Console.WriteLine("\n=== INTEGRARE SERVICIU ASINCRON ===\n");
            
            Console.WriteLine("Simulare procesare asincronă...");
            
            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Task 1 completat");
            });
            
            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(800);
                Console.WriteLine("Task 2 completat");
            });
            
            Console.WriteLine("Toate task-urile asincrone au fost completate.");
        }
        
        static void GenerateReports(ReportGenerator reportGenerator, DataRepository repository)
        {
            Console.WriteLine("\n=== GENERARE RAPOARTE ===\n");
            
            var mealPlan = new MealPlan
            {
                TargetCalories = 2000,
                Date = DateTime.Now
            };
            
            var macros = new Macronutrients
            {
                ProteinGrams = 150,
                CarbsGrams = 200,
                FatGrams = 67
            };
            
            string pdfPath = reportGenerator.GeneratePdfReport("Utilizator Demo", mealPlan, macros);
            string excelPath = reportGenerator.GenerateExcelReport("Utilizator Demo", repository.GetAllFoodItems());
            string statsPath = reportGenerator.GenerateStatisticsReport("Utilizator Demo", 
                DateTime.Now.AddDays(-30), DateTime.Now);
            
            Console.WriteLine($@"
Rapoarte generate:
  📄 PDF:  {pdfPath}
  📊 Excel: {excelPath}
  📈 Stats: {statsPath}
");
        }
        
        static void RunStatisticsDemo(StatisticsCalculator statisticsCalculator)
        {
            Console.WriteLine("\n=== STATISTICI CONSUMATOARE DE TIMP ===\n");
            statisticsCalculator.RunPerformanceBenchmark();
            
            Console.WriteLine("\n=== STATISTICI AVANSATE ===\n");
            var data = new List<double> { 1500, 1600, 1550, 1700, 1650, 1800, 1750, 1900, 1850, 2000 };
            statisticsCalculator.CalculateAdvancedStatistics(data);
        }
    }
}
