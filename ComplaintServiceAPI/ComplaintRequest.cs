using System.ComponentModel.DataAnnotations;
using NLog;

//using NLog;

namespace ComplaintServiceAPI
{
    public class ComplaintRequest
    {
        [Required]
        public string subject { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string emailAddress { get; set; }
        [Required]
        public string productName { get; set; }
    }
    
    public interface ILoggerManager
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogDebugging(string message);
        void LogError(string message);
    }

    public class LoggManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogInformation(string message)
        {
            logger.Info(message);
        }
    
        public void LogWarning(string message)
        {
            logger.Warn(message);
        }
    
        public void LogDebugging(string message)
        {
            logger.Debug(message);
        }
    
        public void LogError(string message)
        {
            logger.Error(message);
        }
    }
}