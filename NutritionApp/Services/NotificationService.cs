using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NutritionApp.Models;

namespace NutritionApp.Services
{
    /// <summary>
    /// Serviciu de notificare cu fire de execuție paralele
    /// Trimite notificări când apar anunțuri/alimente care corespund cerințelor utilizatorului
    /// </summary>
    public class NotificationService
    {
        private readonly Queue<NotificationTask> _notificationQueue;
        private readonly List<Task> _activeTasks;
        private readonly int _maxParallelThreads;
        private bool _isRunning;
        
        public NotificationService(int maxParallelThreads = 4)
        {
            _notificationQueue = new Queue<NotificationTask>();
            _activeTasks = new List<Task>();
            _maxParallelThreads = maxParallelThreads;
            _isRunning = false;
        }
        
        /// <summary>
        /// Adaugă o notificare în coadă pentru procesare
        /// </summary>
        public void AddNotification(UserProfile userProfile, FoodItem foodItem, string userEmail)
        {
            var task = new NotificationTask
            {
                UserProfile = userProfile,
                FoodItem = foodItem,
                UserEmail = userEmail,
                CreatedAt = DateTime.Now
            };
            
            _notificationQueue.Enqueue(task);
            Console.WriteLine($"[NOTIFY] Notificare adăugată pentru: {foodItem.Name}");
            
            // Procesează asincron dacă serviciul este pornit
            if (_isRunning)
            {
                ProcessNotificationsAsync();
            }
        }
        
        /// <summary>
        /// Pornește procesarea notificărilor în fire de execuție paralele
        /// </summary>
        public void Start()
        {
            _isRunning = true;
            Console.WriteLine($"[NOTIFY] Serviciu de notificări pornit (max {_maxParallelThreads} thread-uri)");
        }
        
        /// <summary>
        /// Oprește procesarea notificărilor
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            Console.WriteLine("[NOTIFY] Serviciu de notificări oprit");
        }
        
        /// <summary>
        /// Procesează notificările din coadă folosind fire de execuție paralele
        /// </summary>
        private async void ProcessNotificationsAsync()
        {
            while (_isRunning && _notificationQueue.Count > 0)
            {
                var tasks = new List<Task>();
                
                // Pornește până la _maxParallelThreads fire de execuție
                for (int i = 0; i < _maxParallelThreads && _notificationQueue.Count > 0; i++)
                {
                    var notificationTask = _notificationQueue.Dequeue();
                    tasks.Add(Task.Run(() => ProcessSingleNotification(notificationTask)));
                }
                
                // Așteaptă ca toate thread-urile curente să se termine
                await Task.WhenAll(tasks);
            }
        }
        
        /// <summary>
        /// Procesează o singură notificare (rulează într-un thread separat)
        /// </summary>
        private void ProcessSingleNotification(NotificationTask task)
        {
            try
            {
                Console.WriteLine($"[THREAD {Thread.CurrentThread.ManagedThreadId}] Se procesează notificarea pentru {task.FoodItem.Name}");
                
                // Verifică dacă alimentul corespunde cerințelor utilizatorului
                bool matchesCriteria = CheckIfMatchesCriteria(task.UserProfile, task.FoodItem);
                
                if (matchesCriteria)
                {
                    // Trimite notificarea
                    SendNotification(task);
                    
                    Console.WriteLine($"[THREAD {Thread.CurrentThread.ManagedThreadId}] Notificare trimisă către {task.UserEmail}");
                }
                else
                {
                    Console.WriteLine($"[THREAD {Thread.CurrentThread.ManagedThreadId}] Alimentul nu corespunde cerințelor");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[THREAD {Thread.CurrentThread.ManagedThreadId}] Eroare: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Verifică dacă un aliment corespunde cerințelor utilizatorului
        /// </summary>
        private bool CheckIfMatchesCriteria(UserProfile profile, FoodItem food)
        {
            // Logica de verificare a criteriilor
            // De exemplu: verifică caloriile, macronutrienții, categoria, etc.
            
            // Exemplu: utilizatorul vrea alimente cu proteine mari
            bool highProtein = food.ProteinPer100g >= 10;
            
            // Exemplu: utilizatorul vrea alimente low-carb
            bool lowCarb = food.CarbsPer100g <= 15;
            
            return highProtein || lowCarb;
        }
        
        /// <summary>
        /// Trimite efectiv notificarea (email, push notification, etc.)
        /// </summary>
        private void SendNotification(NotificationTask task)
        {
            // Implementare trimitere notificare
            // Poate fi email, SMS, push notification, etc.
            
            Console.WriteLine($@"
===========================================
NOTIFICARE NOUĂ
===========================================
Către: {task.UserEmail}
Aliment: {task.FoodItem.Name}
Calorii: {task.FoodItem.CaloriesPer100g} kcal/100g
Proteine: {task.FoodItem.ProteinPer100g}g
Data: {task.CreatedAt:dd.MM.yyyy HH:mm:ss}
===========================================");
        }
        
        /// <summary>
        /// Monitorizează apariția de anunțuri noi și notifică utilizatorii
        /// Rulează continuu în background
        /// </summary>
        public async Task MonitorNewAnnouncementsAsync(List<FoodItem> newFoodItems)
        {
            Console.WriteLine("[MONITOR] Se monitorizează anunțurile noi...");
            
            foreach (var foodItem in newFoodItems)
            {
                // Simulare verificare baza de date de utilizatori
                var interestedUsers = GetInterestedUsers(foodItem);
                
                foreach (var user in interestedUsers)
                {
                    AddNotification(user.Profile, foodItem, user.Email);
                }
                
                // Mic delay pentru a nu supraîncărca sistemul
                await Task.Delay(100);
            }
        }
        
        /// <summary>
        /// Obține utilizatorii interesați de un anumit tip de aliment (simulat)
        /// </summary>
        private List<InterestedUser> GetInterestedUsers(FoodItem foodItem)
        {
            // În producție, aceasta ar veni din baza de date
            return new List<InterestedUser>
            {
                new InterestedUser
                {
                    Email = "user1@example.com",
                    Profile = new UserProfile 
                    { 
                        HeightCm = 175, 
                        WeightKg = 70, 
                        Age = 30, 
                        Sex = 'm', 
                        ActivityLevel = UserProfile.ActivityLevel.Moderat 
                    }
                },
                new InterestedUser
                {
                    Email = "user2@example.com",
                    Profile = new UserProfile 
                    { 
                        HeightCm = 165, 
                        WeightKg = 60, 
                        Age = 25, 
                        Sex = 'f', 
                        ActivityLevel = UserProfile.ActivityLevel.Intens 
                    }
                }
            };
        }
    }
    
    /// <summary>
    /// Task individual de notificare
    /// </summary>
    public class NotificationTask
    {
        public UserProfile UserProfile { get; set; }
        public FoodItem FoodItem { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    /// <summary>
    /// Utilizator interesat de anumite tipuri de alimente
    /// </summary>
    public class InterestedUser
    {
        public string Email { get; set; }
        public UserProfile Profile { get; set; }
    }
}
