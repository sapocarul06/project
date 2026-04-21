using System;

namespace NutritionApp.Models
{
    /// <summary>
    /// Model pentru profilul utilizatorului cu setările personale
    /// </summary>
    public class UserProfile
    {
        public double HeightCm { get; set; }      // Înălțime în cm
        public double WeightKg { get; set; }      // Greutate în kg
        public int Age { get; set; }              // Vârstă în ani
        public char Sex { get; set; }             // 'm' sau 'f'
        public ActivityLevel ActivityLevel { get; set; }  // Nivel activitate fizică
        
        /// <summary>
        /// Enum pentru nivelurile de activitate fizică
        /// </summary>
        public enum ActivityLevel
        {
            Sedentar,      // Fără exerciții sau foarte puțin
            Moderat,       // Exerciții moderate 3-5 zile/săptămână
            Intens         // Exerciții intense 6-7 zile/săptămână
        }
        
        /// <summary>
        /// Validează datele profilului
        /// </summary>
        public bool Validate()
        {
            return HeightCm > 0 && HeightCm < 300 &&
                   WeightKg > 0 && WeightKg < 500 &&
                   Age > 0 && Age < 150 &&
                   (Sex == 'm' || Sex == 'f');
        }
        
        public override string ToString()
        {
            return $"Profil: {HeightCm}cm, {WeightKg}kg, {Age}ani, Sex: {(Sex == 'm' ? "Masculin" : "Feminin")}, Activitate: {ActivityLevel}";
        }
    }
}
