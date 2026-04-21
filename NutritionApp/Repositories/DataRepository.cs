using System;
using System.Collections.Generic;
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
            var result = new List<FoodItem>();
            foreach (var f in _foodItems)
            {
                if (f.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    result.Add(f);
            }
            return result;
        }
        
        public List<FoodItem> GetFoodItemsByCategory(string category)
        {
            var result = new List<FoodItem>();
            foreach (var f in _foodItems)
            {
                if (f.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                    result.Add(f);
            }
            return result;
        }
        
        #endregion
        
        #region Meal Plan Operations
        
        public void AddMealPlan(MealPlan plan)
        {
            _mealPlans.Add(plan);
        }
        
        public List<MealPlan> GetMealPlansByDateRange(DateTime start, DateTime end)
        {
            var result = new List<MealPlan>();
            foreach (var m in _mealPlans)
            {
                if (m.Date >= start && m.Date <= end)
                    result.Add(m);
            }
            return result;
        }
        
        #endregion
    }
}
