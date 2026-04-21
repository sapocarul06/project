using System;
using System.Collections.Generic;
using System.Linq;
using NutritionApp.Models;
using NutritionApp.Utils;

namespace NutritionApp.Services
{
    /// <summary>
    /// Serviciu pentru compararea imaginilor și identificarea anunțurilor/alimentelor duplicate
    /// </summary>
    public class ImageComparisonService
    {
        private readonly Dictionary<int, string> _imageHashes;
        
        public ImageComparisonService()
        {
            _imageHashes = new Dictionary<int, string>();
        }
        
        /// <summary>
        /// Adaugă o imagine în baza de date de hash-uri
        /// </summary>
        public void AddImage(int foodId, string imagePath)
        {
            try
            {
                string hash = ImageHash.GeneratePerceptualHash(imagePath);
                
                if (_imageHashes.ContainsKey(foodId))
                    _imageHashes[foodId] = hash;
                else
                    _imageHashes.Add(foodId, hash);
                    
                Console.WriteLine($"[IMAGE] Hash generat pentru alimentul {foodId}: {hash}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IMAGE] Eroare la procesarea imaginii {imagePath}: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Identifică anunțuri/alimente cu imagini identice sau foarte similare
        /// </summary>
        public List<DuplicateGroup> FindDuplicates(List<FoodItem> foodItems, int threshold = 5)
        {
            var duplicates = new List<DuplicateGroup>();
            var processed = new HashSet<int>();
            
            for (int i = 0; i < foodItems.Count; i++)
            {
                if (processed.Contains(foodItems[i].Id))
                    continue;
                    
                var group = new DuplicateGroup { PrimaryItem = foodItems[i] };
                
                for (int j = i + 1; j < foodItems.Count; j++)
                {
                    if (processed.Contains(foodItems[j].Id))
                        continue;
                    
                    // Comparăm hash-urile imaginilor
                    if (foodItems[i].ImageUrl != null && foodItems[j].ImageUrl != null)
                    {
                        try
                        {
                            bool areSimilar = ImageHash.AreImagesSimilar(
                                foodItems[i].ImageUrl, 
                                foodItems[j].ImageUrl, 
                                threshold);
                            
                            if (areSimilar)
                            {
                                group.DuplicateItems.Add(foodItems[j]);
                                processed.Add(foodItems[j].Id);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[IMAGE] Eroare comparare: {ex.Message}");
                        }
                    }
                }
                
                if (group.DuplicateItems.Count > 0)
                {
                    duplicates.Add(group);
                    processed.Add(foodItems[i].Id);
                }
            }
            
            return duplicates;
        }
        
        /// <summary>
        /// Script pentru identificarea automată a anunțurilor duplicate
        /// Poate fi rulat periodic prin cron
        /// </summary>
        public void RunDuplicateDetectionScript()
        {
            Console.WriteLine($"[SCRIPT] Începe scanarea pentru duplicate: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            
            // Aici s-ar încărca toate alimentele din baza de date
            var allFoodItems = LoadAllFoodItems();
            
            var duplicates = FindDuplicates(allFoodItems);
            
            Console.WriteLine($"[SCRIPT] Au fost identificate {duplicates.Count} grupuri de duplicate.");
            
            foreach (var group in duplicates)
            {
                Console.WriteLine($"  - Aliment principal: {group.PrimaryItem.Name} (ID: {group.PrimaryItem.Id})");
                foreach (var dup in group.DuplicateItems)
                {
                    Console.WriteLine($"    * Duplicate: {dup.Name} (ID: {dup.Id})");
                }
            }
        }
        
        /// <summary>
        /// Încarcă toate alimentele din repository (simulat)
        /// </summary>
        private List<FoodItem> LoadAllFoodItems()
        {
            // În producție, aceasta ar veni din baza de date
            return new List<FoodItem>
            {
                new FoodItem { Id = 1, Name = "Măr Roșu", ImageUrl = "Data/images/apple1.jpg" },
                new FoodItem { Id = 2, Name = "Măr Galben", ImageUrl = "Data/images/apple2.jpg" },
                new FoodItem { Id = 3, Name = "Banana Premium", ImageUrl = "Data/images/banana1.jpg" },
                new FoodItem { Id = 4, Name = "Banana Standard", ImageUrl = "Data/images/banana1.jpg" } // Aceeași imagine
            };
        }
    }
    
    /// <summary>
    /// Grup de elemente duplicate
    /// </summary>
    public class DuplicateGroup
    {
        public FoodItem PrimaryItem { get; set; }
        public List<FoodItem> DuplicateItems { get; set; }
        
        public DuplicateGroup()
        {
            DuplicateItems = new List<FoodItem>();
        }
    }
}
