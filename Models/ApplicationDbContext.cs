namespace WebApplication2.Models
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using System.Data;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Rezultati")]

    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext() { }


        private readonly IConfiguration _configuration;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        private IDbConnection DbConnection { get; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("BazaDB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var config = modelBuilder.Entity<Rezultat>();
            config.ToTable("Rezultati");
        }
        
        public DbSet<Rezultat> Rezultatis { get; set; } 
        protected readonly IConfiguration Configuration;

        

        public class Rezultat
        {
            public int Id { get; set; }
            public string Naslov { get; set; }
            public string Html { get; set; }
        }






   
    }
}
