using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace NutritionApp.Utils
{
    /// <summary>
    /// Utilitar pentru calcularea statisticilor consumatoare de timp
    /// Include benchmarking și profiling al operațiunilor
    /// </summary>
    public class StatisticsCalculator
    {
        private readonly Stopwatch _stopwatch;
        
        public StatisticsCalculator()
        {
            _stopwatch = new Stopwatch();
        }
        
        /// <summary>
        /// Măsoară timpul de execuție pentru o operație
        /// </summary>
        public TimeSpan MeasureExecutionTime(Action operation)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            
            operation();
            
            _stopwatch.Stop();
            return _stopwatch.Elapsed;
        }
        
        /// <summary>
        /// Rulează un studiu comparativ de performanță între diferite metode
        /// </summary>
        public void RunPerformanceBenchmark()
        {
            Console.WriteLine(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║              STUDIU COMPARATIV: PERFORMANȚĂ STOCARE DATE                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
");
            
            // Benchmark 1: Operații pe liste
            var listTime = MeasureExecutionTime(() =>
            {
                var list = new List<int>();
                for (int i = 0; i < 10000; i++)
                    list.Add(i);
            });
            Console.WriteLine($"║ List<T> - Adăugare 10k elemente: {listTime.TotalMilliseconds:F2}ms");
            
            // Benchmark 2: Operații pe array
            var arrayTime = MeasureExecutionTime(() =>
            {
                var array = new int[10000];
                for (int i = 0; i < 10000; i++)
                    array[i] = i;
            });
            Console.WriteLine($"║ Array - Inițializare 10k elemente: {arrayTime.TotalMilliseconds:F2}ms");
            
            // Benchmark 3: Operații pe Dictionary
            var dictTime = MeasureExecutionTime(() =>
            {
                var dict = new Dictionary<int, string>();
                for (int i = 0; i < 10000; i++)
                    dict.Add(i, $"Item{i}");
            });
            Console.WriteLine($"║ Dictionary - Adăugare 10k elemente: {dictTime.TotalMilliseconds:F2}ms");
            
            // Benchmark 4: Filtrare manuală (fără LINQ pentru compatibilitate C# 7.3)
            var filterTime = MeasureExecutionTime(() =>
            {
                var numbers = new List<int>();
                for (int i = 0; i < 10000; i++)
                    numbers.Add(i);
                var result = new List<int>();
                foreach (var n in numbers)
                {
                    if (n % 2 == 0)
                        result.Add(n);
                }
            });
            Console.WriteLine($"║ Manual Filter - Filtrare 10k elemente: {filterTime.TotalMilliseconds:F2}ms");
            
            Console.WriteLine(@"
╠══════════════════════════════════════════════════════════════════════════════╣
║  STUDIU COMPARATIV: LOCAL vs SERVER vs SERVERLESS                            ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║  1. STOCARE LOCALĂ (Local Storage / SQLite)                                  ║
║     ───────────────────────────────────────────                              ║
║     ✅ Avantaje:                                                               ║
║        • Latență minimă (acces direct la disk)                               ║
║        • Fără costuri de rețea                                               ║
║        • Funcționează offline                                                ║
║        • Control total asupra datelor                                        ║
║                                                                              ║
║     ❌ Dezavantaje:                                                            ║
║        • Nu e accesibil de pe multiple dispozitive                           ║
║        • Backup manual                                                       ║
║        • Scalabilitate limitată                                              ║
║        • Risc de pierdere a datelor                                          ║
║                                                                              ║
║     📊 Performanță: ~1-5ms per operație                                      ║
║     💰 Cost: Gratuit (doar hardware local)                                   ║
║                                                                              ║
║  2. STOCARE PE SERVER DEDICAT (VPS / Dedicated Server)                       ║
║     ─────────────────────────────────────────────────────                    ║
║     ✅ Avantaje:                                                               ║
║        • Accesibil de oriunde                                                ║
║        • Control asupra configurației                                        ║
║        • Backup automatizat                                                  ║
║        • Scalabilitate verticală posibilă                                    ║
║                                                                              ║
║     ❌ Dezavantaje:                                                            ║
║        • Costuri fixe lunare                                                 ║
║        • Necesită administrare                                               ║
║        • Latență de rețea                                                    ║
║        • Responsabilitatea securității                                       ║
║                                                                              ║
║     📊 Performanță: ~10-50ms per operație (depinde de rețea)                 ║
║     💰 Cost: €10-100/lună (depinde de resurse)                               ║
║                                                                              ║
║  3. ARHITECTURĂ SERVERLESS (Azure Functions / AWS Lambda)                    ║
║     ───────────────────────────────────────────────────────────              ║
║     ✅ Avantaje:                                                               ║
║        • Scalabilitate automată                                              ║
║        • Plătești doar pentru ce folosești                                   ║
║        • Fără administrare server                                            ║
║        • Integrare ușoară cu alte servicii cloud                             ║
║                                                                              ║
║     ❌ Dezavantaje:                                                            ║
║        • Cold start latency                                                  ║
║        • Limitări de timp execuție                                           ║
║        • Debugging mai complex                                               ║
║        • Vendor lock-in                                                      ║
║                                                                              ║
║     📊 Performanță: ~50-500ms per operație (incluzând cold start)            ║
║     💰 Cost: €0.000016/GB-second (variabil)                                  ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║  RECOMANDARE PENTRU NUTRITIONAPP:                                            ║
║  → Dezvoltare/Test: Stocare locală (SQLite)                                  ║
║  → Producție mică: Server VPS (€10-20/lună)                                  ║
║  → Producție mare: Arhitectură hibridă (Server + Serverless pentru task-uri) ║
╚══════════════════════════════════════════════════════════════════════════════╝
");
        }
        
        /// <summary>
        /// Calculează statistici avansate pentru un set de date
        /// </summary>
        public void CalculateAdvancedStatistics(List<double> data)
        {
            if (data == null || data.Count == 0)
            {
                Console.WriteLine("Nu există date pentru analiză.");
                return;
            }
            
            double sum = 0;
            foreach (var value in data)
                sum += value;
            
            double mean = sum / data.Count;
            
            // Calcul mediană
            var sorted = new List<double>(data);
            sorted.Sort();
            double median = sorted.Count % 2 == 0 
                ? (sorted[sorted.Count / 2 - 1] + sorted[sorted.Count / 2]) / 2 
                : sorted[sorted.Count / 2];
            
            // Calcul deviație standard
            double variance = 0;
            foreach (var value in data)
                variance += Math.Pow(value - mean, 2);
            variance /= data.Count;
            double stdDev = Math.Sqrt(variance);
            
            Console.WriteLine($@"
Statistici Avansate:
─────────────────────
Count: {data.Count}
Sum: {sum:F2}
Mean (Medie): {mean:F2}
Median: {median:F2}
Min: {sorted[0]:F2}
Max: {sorted[sorted.Count - 1]:F2}
Std Dev: {stdDev:F2}
");
        }
    }
}
