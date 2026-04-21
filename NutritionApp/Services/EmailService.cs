using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NutritionApp.Services
{
    /// <summary>
    /// Serviciu pentru trimiterea de emailuri
    /// Include funcționalitate pentru programarea trimiterilor prin cron
    /// </summary>
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        
        public EmailService(string smtpServer, int smtpPort, string username, string password, string fromEmail)
        {
            _smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };
            _fromEmail = fromEmail;
        }
        
        /// <summary>
        /// Trimite un email simplu
        /// </summary>
        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body);
                mailMessage.IsBodyHtml = true;
                
                _smtpClient.Send(mailMessage);
                Console.WriteLine($"[EMAIL] Email trimis către {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL] Eroare la trimitere: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Trimite un email asincron
        /// </summary>
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body);
                mailMessage.IsBodyHtml = true;
                
                await _smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine($"[EMAIL] Email trimis asincron către {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL] Eroare la trimitere asincronă: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Trimite materiale promoționale utilizatorilor
        /// Poate fi apelat din cron job zilnic/săptămânal
        /// </summary>
        public void SendPromotionalMaterials(string toEmail, string promotionalContent)
        {
            string subject = "Oferte Speciale - Planul Tău Alimentar";
            string body = $@"
<html>
<body style='font-family: Arial, sans-serif;'>
    <h2>Bună!</h2>
    <p>Avem oferte speciale pentru tine:</p>
    <div style='background-color: #f0f0f0; padding: 20px; margin: 20px 0;'>
        {promotionalContent}
    </div>
    <p>Nu rata aceste oportunități!</p>
    <p>Echipa NutritionApp</p>
</body>
</html>";
            
            SendEmail(toEmail, subject, body);
        }
        
        /// <summary>
        /// Script pentru trimiterea automată de emailuri promoționale
        /// Configurat să ruleze prin cron la intervale regulate
        /// </summary>
        public void CronPromotionalEmailScript()
        {
            Console.WriteLine($"[CRON] Începe trimiterea emailurilor promoționale: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            
            // Lista de utilizatori cărora li se trimit emailuri
            var users = GetUsersForPromotionalEmail();
            
            foreach (var user in users)
            {
                try
                {
                    string promotionalContent = GeneratePromotionalContent(user);
                    SendPromotionalMaterials(user.Email, promotionalContent);
                    
                    // Mic delay pentru a nu fi marcat ca spam
                    System.Threading.Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CRON] Eroare la trimitere către {user.Email}: {ex.Message}");
                }
            }
            
            Console.WriteLine("[CRON] Trimitere emailuri promoționale completă.");
        }
        
        /// <summary>
        /// Obține lista de utilizatori pentru emailuri promoționale (simulat)
        /// </summary>
        private PromotionalUser[] GetUsersForPromotionalEmail()
        {
            // În producție, aceasta ar veni din baza de date
            return new[]
            {
                new PromotionalUser { Email = "user1@example.com", Name = "Ion Popescu" },
                new PromotionalUser { Email = "user2@example.com", Name = "Maria Ionescu" },
                new PromotionalUser { Email = "user3@example.com", Name = "Andrei Radu" }
            };
        }
        
        /// <summary>
        /// Generează conținut promoțional personalizat
        /// </summary>
        private string GeneratePromotionalContent(PromotionalUser user)
        {
            return $@"
                <h3>Dragă {user.Name},</h3>
                <ul>
                    <li>🥗 Planuri alimentare personalizate cu 20% reducere</li>
                    <li>📊 Rapoarte nutriționale detaliate</li>
                    <li>🏆 Acces premium la rețete exclusive</li>
                </ul>
                <p><strong>Ofertă valabilă până la sfârșitul lunii!</strong></p>
            ";
        }
    }
    
    /// <summary>
    /// Model pentru utilizator în contextul emailurilor promoționale
    /// </summary>
    public class PromotionalUser
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
