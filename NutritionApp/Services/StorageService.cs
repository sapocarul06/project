using System;
using System.IO;
using System.Collections.Generic;

namespace NutritionApp.Services
{
    /// <summary>
    /// Serviciu pentru gestionarea stocării imaginilor
    /// Include studiu comparativ între stocarea pe server vs baza de date
    /// </summary>
    public class StorageService
    {
        private readonly string _fileStoragePath;
        private readonly StorageStrategy _strategy;
        
        public enum StorageStrategy
        {
            FileSystem,     // Stocare în sistemul de fișiere
            Database,       // Stocare în baza de date (BLOB)
            Cloud           // Stocare în cloud (Azure Blob, AWS S3, etc.)
        }
        
        public StorageService(StorageStrategy strategy = StorageStrategy.FileSystem, string storagePath = "Data/Images")
        {
            _strategy = strategy;
            _fileStoragePath = storagePath;
            
            if (_strategy == StorageStrategy.FileSystem && !Directory.Exists(_fileStoragePath))
                Directory.CreateDirectory(_fileStoragePath);
        }
        
        /// <summary>
        /// Stochează o imagine conform strategiei alese
        /// </summary>
        public string StoreImage(int foodId, byte[] imageBytes, string originalFileName)
        {
            switch (_strategy)
            {
                case StorageStrategy.FileSystem:
                    return StoreInFileSystem(foodId, imageBytes, originalFileName);
                case StorageStrategy.Database:
                    return StoreInDatabase(foodId, imageBytes, originalFileName);
                case StorageStrategy.Cloud:
                    return StoreInCloud(foodId, imageBytes, originalFileName);
                default:
                    throw new ArgumentException("Strategie de stocare invalidă");
            }
        }
        
        /// <summary>
        /// Stochează imaginea în sistemul de fișiere
        /// Avantaje: Acces rapid, cost redus, ușor de implementat
        /// Dezavantaje: Backup mai complicat, nu e tranzacțional
        /// </summary>
        private string StoreInFileSystem(int foodId, byte[] imageBytes, string originalFileName)
        {
            string extension = Path.GetExtension(originalFileName);
            string fileName = $"food_{foodId}_{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(_fileStoragePath, fileName);
            
            File.WriteAllBytes(filePath, imageBytes);
            
            Console.WriteLine($"[STORAGE] Imagine stocată în FileSystem: {filePath}");
            return filePath;
        }
        
        /// <summary>
        /// Stochează imaginea în baza de date ca BLOB
        /// Avantaje: Backup unitar, tranzacționalitate, consistență
        /// Dezavantaje: Performanță mai scăzută, dimensiune DB crescută
        /// </summary>
        private string StoreInDatabase(int foodId, byte[] imageBytes, string originalFileName)
        {
            // Simulare stocare în database
            // În producție: INSERT INTO FoodImages (FoodId, ImageData, FileName) VALUES (...)
            
            string dbReference = $"DB:BLOB:{foodId}:{Guid.NewGuid()}";
            int sizeKB = imageBytes.Length / 1024;
            
            Console.WriteLine($"[STORAGE] Imagine stocată în Database: {dbReference} ({sizeKB} KB)");
            Console.WriteLine($"[STORAGE] Notă: Stocarea în DB poate afecta performanța la volume mari");
            
            return dbReference;
        }
        
        /// <summary>
        /// Stochează imaginea în cloud
        /// Avantaje: Scalabilitate, CDN, backup automat
        /// Dezavantaje: Costuri recurente, dependență de provider
        /// </summary>
        private string StoreInCloud(int foodId, byte[] imageBytes, string originalFileName)
        {
            // Simulare stocare în cloud (Azure Blob Storage / AWS S3)
            string cloudUrl = $"https://storage.cloud.example.com/nutrition-app/food_{foodId}_{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            
            Console.WriteLine($"[STORAGE] Imagine stocată în Cloud: {cloudUrl}");
            Console.WriteLine($"[STORAGE] CDN enabled pentru livrare rapidă");
            
            return cloudUrl;
        }
        
        /// <summary>
        /// Încarcă o imagine din storage
        /// </summary>
        public byte[] LoadImage(string imageReference)
        {
            if (imageReference.StartsWith("DB:"))
            {
                // Încărcare din database
                Console.WriteLine("[STORAGE] Încărcare din Database...");
                return LoadFromDatabase(imageReference);
            }
            else if (imageReference.StartsWith("http"))
            {
                // Încărcare din cloud
                Console.WriteLine("[STORAGE] Încărcare din Cloud...");
                return LoadFromCloud(imageReference);
            }
            else
            {
                // Încărcare din filesystem
                Console.WriteLine("[STORAGE] Încărcare din FileSystem...");
                return File.ReadAllBytes(imageReference);
            }
        }
        
        private byte[] LoadFromDatabase(string dbReference)
        {
            // Simulare încărcare din DB
            return new byte[1024 * 50]; // 50KB mock
        }
        
        private byte[] LoadFromCloud(string cloudUrl)
        {
            // Simulare încărcare din cloud
            return new byte[1024 * 50]; // 50KB mock
        }
        
        /// <summary>
        /// Studiu comparativ: FileSystem vs Database vs Cloud
        /// </summary>
        public void PrintStorageComparisonStudy()
        {
            Console.WriteLine(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                    STUDIU COMPARATIV: STOCARE IMAGINI                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║  1. STOCARE ÎN SISTEMUL DE FIȘIERE (FileSystem)                              ║
║     ─────────────────────────────────────────────                            ║
║     ✅ Avantaje:                                                               ║
║        • Performanță ridicată la citire/scriere                              ║
║        • Cost minim (doar spațiu disk)                                       ║
║        • Implementare simplă                                                 ║
║        • Ușor de gestionat cu CDN extern                                     ║
║                                                                              ║
║     ❌ Dezavantaje:                                                            ║
║        • Backup separat de baza de date                                      ║
║        • Nu beneficiază de tranzacționalitate DB                             ║
║        • Gestionare mai dificilă în medii distribuite                        ║
║        • Potențiale probleme de permisiuni                                   ║
║                                                                              ║
║     📊 Recomandat pentru: Aplicații mici-medii, buget limitat                ║
║                                                                              ║
║  2. STOCARE ÎN BAZA DE DATE (BLOB)                                           ║
║     ────────────────────────────────────────                                 ║
║     ✅ Avantaje:                                                               ║
║        • Backup unitar cu datele aplicației                                  ║
║        • Tranzacționalitate garantată                                        ║
║        • Consistență referențială                                            ║
║        • Securitate centralizată                                             ║
║                                                                              ║
║     ❌ Dezavantaje:                                                            ║
║        • Performanță scăzută la imagini mari                                 ║
║        • Crește semnificativ dimensiunea DB                                  ║
║        • Backup/restore mai lent                                             ║
║        • Costuri mai mari de storage pe DB                                   ║
║                                                                              ║
║     📊 Recomandat pentru: Imagini mici (< 100KB), volum redus                ║
║                                                                              ║
║  3. STOCARE ÎN CLOUD (Azure Blob / AWS S3)                                   ║
║     ───────────────────────────────────────────────                          ║
║     ✅ Avantaje:                                                               ║
║        • Scalabilitate infinită                                              ║
║        • CDN integrat pentru livrare rapidă                                  ║
║        • Backup și redundanță automate                                       ║
║        • Costuri proporționale cu utilizarea                                 ║
║        • Procesare imagine (resize, optimizare) built-in                     ║
║                                                                              ║
║     ❌ Dezavantaje:                                                            ║
║        • Costuri recurente lunare                                            ║
║        • Dependență de provider extern                                       ║
║        • Latență potențială (fără CDN local)                                 ║
║        • Complexitate la configurare                                         ║
║                                                                              ║
║     📊 Recomandat pentru: Aplicații enterprise, trafic mare                  ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║  RECOMANDAREA NOASTRĂ PENTRU NUTRITIONAPP:                                   ║
║  → Folosiți Cloud Storage (Azure Blob Storage) cu CDN                        ║
║  → Alternativ: FileSystem + CDN extern (CloudFlare) pentru buget redus       ║
╚══════════════════════════════════════════════════════════════════════════════╝
");
        }
    }
}
