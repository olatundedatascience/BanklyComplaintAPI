using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ComplaintAPI.DbContext
{
    public class Complaint
    {
        [Key]
        public long Id { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string phoneNumber { get; set; }
        public string emailAddress { get; set; }
        public string productName { get; set; }
    }

    public class ComplaintDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ComplaintDbContext(DbContextOptions<ComplaintDbContext> opts):base(opts)
        {
            
        }
        
        public DbSet<Complaint> Complaints { get; set; }
    }
}