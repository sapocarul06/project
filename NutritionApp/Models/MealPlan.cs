using System;
using System.Collections.Generic;

namespace NutritionApp.Models
{
    /// <summary>
    /// Model pentru un plan alimentar zilnic
    /// </summary>
    public class MealPlan
    {
        public DateTime Date { get; set; }
        public int TargetCalories { get; set; }
        public List<Meal> Meals { get; set; }
        
        public MealPlan()
        {
            Date = DateTime.Now;
            Meals = new List<Meal>();
        }
        
        public void AddMeal(Meal meal)
        {
            Meals.Add(meal);
        }
        
        public int TotalCalories
        {
            get
            {
                int total = 0;
                foreach (var meal in Meals)
                {
                    total += meal.Calories;
                }
                return total;
            }
        }
        
        public override string ToString()
        {
            return $"Plan alimentar pentru {Date:dd.MM.yyyy} - Țintă: {TargetCalories} kcal, Total: {TotalCalories} kcal";
        }
    }
    
    /// <summary>
    /// Model pentru o masă individuală
    /// </summary>
    public class Meal
    {
        public string Name { get; set; }
        public MealType Type { get; set; }
        public List<FoodItem> Foods { get; set; }
        
        public Meal()
        {
            Foods = new List<FoodItem>();
        }
        
        public int Calories
        {
            get
            {
                int total = 0;
                foreach (var food in Foods)
                {
                    total += (int)food.CaloriesPer100g;
                }
                return total;
            }
        }
    }
    
    /// <summary>
    /// Tipuri de mese
    /// </summary>
    public enum MealType
    {
        MicDejun,
        Gustare1,
        Pranz,
        Gustare2,
        Cina
    }
}
