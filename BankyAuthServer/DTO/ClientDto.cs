using System.ComponentModel.DataAnnotations;

namespace BankyAuthServer.DTO
{
    public class ClientDto
    {
        
        public string clientId { get; set; }
       
        public string clientSecret { get; set; }
        public string clientDescription { get; set; }
        public string clientUri { get; set; }
        
        public string clientName { get; set; }
    }

    public class UserClientKey
    {
        [Key]
        public long id { get; set; }
        public string userId { get; set; }
        public string claimId { get; set; }
    }
    public class UserDto
    {
        public string userId { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }
        public string clientUri { get; set; }
        [Required]
        public string clientName { get; set; }
    }
}