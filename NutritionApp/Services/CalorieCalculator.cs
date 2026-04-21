using System;
using NutritionApp.Models;

namespace NutritionApp.Services
{
    /// <summary>
    /// Serviciu pentru calcularea necesarului caloric
    /// Folosește formula Mifflin-St Jeor pentru BMR și factori de activitate
    /// </summary>
    public class CalorieCalculator
    {
        /// <summary>
        /// Calculează Rata Metabolică Bazală (BMR) folosind formula Mifflin-St Jeor
        /// </summary>
        public static double CalculateBMR(UserProfile profile)
        {
            if (!profile.Validate())
                throw new ArgumentException("Profil invalid!");
            
            // Formula Mifflin-St Jeor
            // Pentru bărbați: BMR = 10×greutate(kg) + 6.25×înălțime(cm) - 5×vârstă(ani) + 5
            // Pentru femei: BMR = 10×greutate(kg) + 6.25×înălțime(cm) - 5×vârstă(ani) - 161
            
            double bmr = 10 * profile.WeightKg + 6.25 * profile.HeightCm - 5 * profile.Age;
            
            if (profile.Sex == 'm')
                bmr += 5;
            else if (profile.Sex == 'f')
                bmr -= 161;
            
            return bmr;
        }
        
        /// <summary>
        /// Calculează necesarul caloric total (TDEE) în funcție de nivelul de activitate
        /// </summary>
        public static double CalculateTDEE(UserProfile profile)
        {
            double bmr = CalculateBMR(profile);
            
            // Factori de activitate
            double activityMultiplier = 1.2; // Default sedentar
            
            switch (profile.ActivityLevel)
            {
                case UserProfile.ActivityLevel.Sedentar:
                    activityMultiplier = 1.2;
                    break;
                case UserProfile.ActivityLevel.Moderat:
                    activityMultiplier = 1.55;
                    break;
                case UserProfile.ActivityLevel.Intens:
                    activityMultiplier = 1.725;
                    break;
            }
            
            return bmr * activityMultiplier;
        }
        
        /// <summary>
        /// Calculează necesarul caloric pentru deficit (pierdere în greutate)
        /// Deficit standard de 500 kcal/zi pentru pierdere de ~0.5kg/săptămână
        /// </summary>
        public static int CalculateCalorieDeficit(UserProfile profile, int deficitAmount = 500)
        {
            double tdee = CalculateTDEE(profile);
            int deficitCalories = (int)(tdee - deficitAmount);
            
            // Asigurăm un minim de 1200 kcal pentru femei și 1500 kcal pentru bărbați
            int minimumCalories = profile.Sex == 'f' ? 1200 : 1500;
            
            return Math.Max(deficitCalories, minimumCalories);
        }
        
        /// <summary>
        /// Calculează macronutrienții recomandați (proteine, carbohidrați, grăsimi)
        /// </summary>
        public static Macronutrients CalculateMacros(UserProfile profile, int targetCalories)
        {
            // Distribuție recomandată: 30% proteine, 40% carbohidrați, 30% grăsimi
            double proteinCalories = targetCalories * 0.30;
            double carbCalories = targetCalories * 0.40;
            double fatCalories = targetCalories * 0.30;
            
            // 1g proteine = 4 kcal, 1g carbohidrați = 4 kcal, 1g grăsimi = 9 kcal
            return new Macronutrients
            {
                ProteinGrams = (int)(proteinCalories / 4),
                CarbsGrams = (int)(carbCalories / 4),
                FatGrams = (int)(fatCalories / 9)
            };
        }
    }
    
    /// <summary>
    /// Model pentru macronutrienți
    /// </summary>
    public class Macronutrients
    {
        public int ProteinGrams { get; set; }
        public int CarbsGrams { get; set; }
        public int FatGrams { get; set; }
        
        public override string ToString()
        {
            return $"Proteine: {ProteinGrams}g, Carbohidrați: {CarbsGrams}g, Grăsimi: {FatGrams}g";
        }
    }
}
