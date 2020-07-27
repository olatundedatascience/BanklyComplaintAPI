using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ComplaintServiceAPI.ComplaintServiceAPIContext
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
        public bool status { get; set; }
    }

    public class ComplaintDbContext :DbContext
    {
        public ComplaintDbContext(DbContextOptions<ComplaintDbContext> opts):base(opts)
        {
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Data Source = FCMB-IT-L16582\TUNDE;database=BanklyTestApp;User Id=rib_details;Password=adejORbl@q9000");
            }
            
            base.OnConfiguring(optionsBuilder);
        }
        
        public DbSet<Complaint> Complaints { get; set; }
    }
    
    public class ComplaintDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ComplaintDbContext>
    {
        public ComplaintDbContext CreateDbContext(string[] args)
        {
            
            var optionsBuilder = new DbContextOptionsBuilder<ComplaintDbContext>();
            optionsBuilder.UseSqlServer(@"Data Source = FCMB-IT-L16582\TUNDE;database=BanklyTestApp;User Id=rib_details;Password=adejORbl@q9000");
            return new ComplaintDbContext(optionsBuilder.Options);
        }
    }
    
}