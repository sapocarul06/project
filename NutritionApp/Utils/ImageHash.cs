using System;
using System.Security.Cryptography;
using System.IO;

namespace NutritionApp.Utils
{
    /// <summary>
    /// Utilitar pentru generarea hash-urilor de imagini (perceptual hash)
    /// Folosit pentru identificarea imaginilor identice sau similare
    /// </summary>
    public class ImageHash
    {
        /// <summary>
        /// Generează un hash perceptual (pHash) pentru o imagine
        /// Returnează un hash de 64 biți sub formă de string hexazecimal
        /// </summary>
        public static string GeneratePerceptualHash(byte[] imageBytes)
        {
            // Implementare simplificată a algoritmului pHash
            // În producție ar necesita procesare reală a imaginii
            
            // Pasul 1: Redimensionare la 32x32
            // Pasul 2: Conversie la grayscale
            // Pasul 3: Aplicare DCT (Discrete Cosine Transform)
            // Pasul 4: Calcul media valorilor (excluzând primul element)
            // Pasul 5: Generare hash bazat pe compararea cu media
            
            // Pentru demo, generăm un hash bazat pe conținutul byte array
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(imageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 16);
            }
        }
        
        /// <summary>
        /// Generează hash pentru un fișier imagine
        /// </summary>
        public static string GeneratePerceptualHash(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("Imaginea nu există", imagePath);
            
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return GeneratePerceptualHash(imageBytes);
        }
        
        /// <summary>
        /// Calculează distanța Hamming între două hash-uri
        /// Returnează numărul de biți diferiți
        /// </summary>
        public static int CalculateHammingDistance(string hash1, string hash2)
        {
            if (hash1.Length != hash2.Length)
                throw new ArgumentException("Hash-urile trebuie să aibă aceeași lungime");
            
            int distance = 0;
            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    distance++;
            }
            
            return distance;
        }
        
        /// <summary>
        /// Verifică dacă două imagini sunt identice sau foarte similare
        /// Threshold: numărul maxim de biți diferiți acceptat (default 5)
        /// </summary>
        public static bool AreImagesSimilar(string hash1, string hash2, int threshold = 5)
        {
            int distance = CalculateHammingDistance(hash1, hash2);
            return distance <= threshold;
        }
        
        /// <summary>
        /// Compară două fișiere imagine pentru similaritate
        /// </summary>
        public static bool AreImagesSimilar(string imagePath1, string imagePath2, int threshold = 5)
        {
            string hash1 = GeneratePerceptualHash(imagePath1);
            string hash2 = GeneratePerceptualHash(imagePath2);
            return AreImagesSimilar(hash1, hash2, threshold);
        }
    }
}
