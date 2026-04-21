using System;

namespace NutritionApp.Models
{
    /// <summary>
    /// Model pentru un aliment din baza de date
    /// </summary>
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CaloriesPer100g { get; set; }
        public double ProteinPer100g { get; set; }
        public double CarbsPer100g { get; set; }
        public double FatPer100g { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string Source { get; set; }  // Provider-ul (ex: Kaggle, USDA, etc.)
        public DateTime LastUpdated { get; set; }
        
        public FoodItem()
        {
            LastUpdated = DateTime.Now;
        }
        
        public override string ToString()
        {
            return $"{Name} - {CaloriesPer100g} kcal/100g (Proteine: {ProteinPer100g}g, Carbohidrați: {CarbsPer100g}g, Grăsimi: {FatPer100g}g)";
        }
    }
}
