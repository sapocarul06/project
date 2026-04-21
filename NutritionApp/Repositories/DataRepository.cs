using System;
using System.Collections.Generic;
using System.Linq;
using NutritionApp.Models;

namespace NutritionApp.Repositories
{
    /// <summary>
    /// Repository pentru gestionarea datelor aplicației
    /// </summary>
    public class DataRepository
    {
        private readonly List<UserProfile> _users;
        private readonly List<FoodItem> _foodItems;
        private readonly List<MealPlan> _mealPlans;
        
        public DataRepository()
        {
            _users = new List<UserProfile>();
            _foodItems = new List<FoodItem>();
            _mealPlans = new List<MealPlan>();
        }
        
        #region User Operations
        
        public void AddUser(UserProfile user)
        {
            _users.Add(user);
        }
        
        public List<UserProfile> GetAllUsers()
        {
            return _users;
        }
        
        #endregion
        
        #region Food Item Operations
        
        public void AddFoodItem(FoodItem item)
        {
            _foodItems.Add(item);
        }
        
        public List<FoodItem> GetAllFoodItems()
        {
            return _foodItems;
        }
        
        public List<FoodItem> SearchFoodItems(string query)
        {
            return _foodItems
                .Where(f => f.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        public List<FoodItem> GetFoodItemsByCategory(string category)
        {
            return _foodItems
                .Where(f => f.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        #endregion
        
        #region Meal Plan Operations
        
        public void AddMealPlan(MealPlan plan)
        {
            _mealPlans.Add(plan);
        }
        
        public List<MealPlan> GetMealPlansByDateRange(DateTime start, DateTime end)
        {
            return _mealPlans
                .Where(m => m.Date >= start && m.Date <= end)
                .ToList();
        }
        
        #endregion
    }
}
