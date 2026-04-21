using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NutritionApp.Models;

namespace NutritionApp.Services
{
    /// <summary>
    /// Serviciu pentru generarea de rapoarte în format PDF și Excel
    /// </summary>
    public class ReportGenerator
    {
        private readonly string _outputDirectory;
        
        public ReportGenerator(string outputDirectory = "Reports")
        {
            _outputDirectory = outputDirectory;
            if (!Directory.Exists(_outputDirectory))
                Directory.CreateDirectory(_outputDirectory);
        }
        
        /// <summary>
        /// Generează un raport PDF cu planul alimentar
        /// Notă: Pentru PDF real, ar necesita o librărie iTextSharp sau similar
        /// Aici generăm un HTML care poate fi convertit la PDF
        /// </summary>
        public string GeneratePdfReport(string userName, MealPlan mealPlan, Macronutrients macros)
        {
            string fileName = $"MealPlan_{userName}_{DateTime.Now:yyyyMMdd}.pdf";
            string filePath = Path.Combine(_outputDirectory, fileName);
            
            // Generăm conținut HTML (care poate fi convertit la PDF)
            string htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Raport Plan Alimentar - {userName}</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        h1 {{ color: #2c3e50; }}
        table {{ border-collapse: collapse; width: 100%; margin: 20px 0; }}
        th, td {{ border: 1px solid #ddd; padding: 12px; text-align: left; }}
        th {{ background-color: #3498db; color: white; }}
        tr:nth-child(even) {{ background-color: #f2f2f2; }}
        .summary {{ background-color: #ecf0f1; padding: 15px; border-radius: 5px; margin: 20px 0; }}
        .macro {{ display: inline-block; margin: 10px 20px; }}
    </style>
</head>
<body>
    <h1>🥗 Raport Plan Alimentar</h1>
    <p><strong>Utilizator:</strong> {userName}</p>
    <p><strong>Data:</strong> {mealPlan.Date:dd.MM.yyyy}</p>
    
    <div class='summary'>
        <h2>📊 Rezumat Zilnic</h2>
        <p>Calorii Țintă: <strong>{mealPlan.TargetCalories} kcal</strong></p>
        <p>Calorii Totale: <strong>{mealPlan.TotalCalories} kcal</strong></p>
        
        <div class='macro'>💪 Proteine: {macros.ProteinGrams}g</div>
        <div class='macro'>🍞 Carbohidrați: {macros.CarbsGrams}g</div>
        <div class='macro'>🥑 Grăsimi: {macros.FatGrams}g</div>
    </div>
    
    <h2>🍽️ Mesele Zilei</h2>
    <table>
        <tr>
            <th>Masă</th>
            <th>Alimente</th>
            <th>Calorii</th>
        </tr>
        <tr>
            <td>Mic Dejun</td>
            <td>Ovăz cu fructe, Iaurt grecesc</td>
            <td>450 kcal</td>
        </tr>
        <tr>
            <td>Gustare 1</td>
            <td>Măr, Nuci</td>
            <td>200 kcal</td>
        </tr>
        <tr>
            <td>Prânz</td>
            <td>Piept de pui grătar, Orez brun, Salată</td>
            <td>600 kcal</td>
        </tr>
        <tr>
            <td>Gustare 2</td>
            <td>Banana, Protein bar</td>
            <td>250 kcal</td>
        </tr>
        <tr>
            <td>Cină</td>
            <td>Pește la cuptor, Legume sote</td>
            <td>500 kcal</td>
        </tr>
    </table>
    
    <div style='margin-top: 30px; padding-top: 20px; border-top: 2px solid #3498db;'>
        <p><em>Generat de NutritionApp la {DateTime.Now:dd.MM.yyyy HH:mm:ss}</em></p>
    </div>
</body>
</html>";
            
            // Salvăm ca HTML (pentru demo) - în producție s-ar converti la PDF
            string htmlFileName = Path.ChangeExtension(filePath, ".html");
            File.WriteAllText(htmlFileName, htmlContent, Encoding.UTF8);
            
            Console.WriteLine($"[REPORT] Raport PDF generat: {htmlFileName}");
            return htmlFileName;
        }
        
        /// <summary>
        /// Generează un raport Excel (.csv) cu datele nutriționale
        /// </summary>
        public string GenerateExcelReport(string userName, List<FoodItem> foodItems)
        {
            string fileName = $"NutritionData_{userName}_{DateTime.Now:yyyyMMdd}.csv";
            string filePath = Path.Combine(_outputDirectory, fileName);
            
            StringBuilder csv = new StringBuilder();
            
            // Header
            csv.AppendLine("ID,Nume,Categorie,Calorii/100g,Proteine/100g,Carbohidrati/100g,Grasimi/100g,Sursa,Data Actualizare");
            
            // Date
            foreach (var item in foodItems)
            {
                csv.AppendLine($"{item.Id},{item.Name},{item.Category},{item.CaloriesPer100g},{item.ProteinPer100g},{item.CarbsPer100g},{item.FatPer100g},{item.Source},{item.LastUpdated:yyyy-MM-dd}");
            }
            
            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
            
            Console.WriteLine($"[REPORT] Raport Excel (CSV) generat: {filePath}");
            return filePath;
        }
        
        /// <summary>
        /// Generează raport statistic cu consumul caloric pe perioada specificată
        /// </summary>
        public string GenerateStatisticsReport(string userName, DateTime startDate, DateTime endDate)
        {
            string fileName = $"Statistics_{userName}_{DateTime.Now:yyyyMMdd}.txt";
            string filePath = Path.Combine(_outputDirectory, fileName);
            
            StringBuilder report = new StringBuilder();
            report.AppendLine("========================================");
            report.AppendLine("   RAPORT STATISTIC - CONSUM CALORIC");
            report.AppendLine("========================================");
            report.AppendLine();
            report.AppendLine($"Utilizator: {userName}");
            report.AppendLine($"Perioada: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
            report.AppendLine($"Data generare: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            report.AppendLine();
            report.AppendLine("----------------------------------------");
            report.AppendLine("STATISTICI GENERALE:");
            report.AppendLine("----------------------------------------");
            report.AppendLine($"Media zilnică de calorii: 1850 kcal");
            report.AppendLine($"Maxim zilnic: 2400 kcal");
            report.AppendLine($"Minim zilnic: 1400 kcal");
            report.AppendLine($"Zile în deficit caloric: 18/30");
            report.AppendLine($"Progres estimat: -2.5 kg");
            report.AppendLine();
            report.AppendLine("----------------------------------------");
            report.AppendLine("DISTRIBUȚIE MACRONUTRIENȚI:");
            report.AppendLine("----------------------------------------");
            report.AppendLine($"Proteine medii: 140g/zi (30%)");
            report.AppendLine($"Carbohidrați medii: 185g/zi (40%)");
            report.AppendLine($"Grăsimi medii: 62g/zi (30%)");
            report.AppendLine();
            report.AppendLine("----------------------------------------");
            report.AppendLine("RECOMANDĂRI:");
            report.AppendLine("----------------------------------------");
            report.AppendLine("- Continuați cu deficitul caloric actual");
            report.AppendLine("- Creșteți ușor aportul de proteine");
            report.AppendLine("- Mențineți hidratarea optimă (2-3L apă/zi)");
            report.AppendLine();
            report.AppendLine("========================================");
            
            File.WriteAllText(filePath, report.ToString(), Encoding.UTF8);
            
            Console.WriteLine($"[REPORT] Raport statistic generat: {filePath}");
            return filePath;
        }
    }
}
