using API_WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace API_WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        //public DbSet<Models.User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        
        //public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().ToTable("KLS_SML_CUSTOMER");
            modelBuilder.Entity<Customer>().Property(c => c.ID).HasColumnName("ID");
            modelBuilder.Entity<Customer>().Property(c => c.CIF_NUMBER).HasColumnName("CIF_NUMBER");
            modelBuilder.Entity<Customer>().Property(c => c.CUSTOMER_TYPE).HasColumnName("CUSTOMER_TYPE");
            modelBuilder.Entity<Customer>().Property(c => c.CUSTOMER_NAME).HasColumnName("CUSTOMER_NAME");
            modelBuilder.Entity<Customer>().Property(c => c.CREATE_DATETIME).HasColumnName("CREATE_DATETIME");
            modelBuilder.Entity<Customer>().Property(c => c.DATA_DATE).HasColumnName("DATA_DATE");
            modelBuilder.Entity<Customer>().Ignore(c => c.CUS_TITLE);
            modelBuilder.Entity<Customer>().Ignore(c => c.CUS_FIRST);
            modelBuilder.Entity<Customer>().Ignore(c => c.CUS_LAST);
            modelBuilder.Entity<Customer>().Ignore(c => c.NEXT_ANN_REV_DATE);

            //modelBuilder.Entity<CIM_CIMD110>().ToTable("CIM_CIMD110");
            //modelBuilder.Entity<CIM_CIMD110>().Property(c => c.CIF).HasColumnName("CIF");
            /*modelBuilder.Entity<CIM_CIMD110>().Property(d => d.CIF).HasColumnName("CIF");
            modelBuilder.Entity<CIM_CIMD110>().Property(d => d.CUS_TITLE).HasColumnName("CUS_TITLE");
            modelBuilder.Entity<CIM_CIMD110>().Property(c => c.CUS_FIRST).HasColumnName("CUS_FIRST");
            modelBuilder.Entity<CIM_CIMD110>().Property(c => c.CUS_LAST).HasColumnName("CUS_LAST");
            modelBuilder.Entity<CIM_CIMD110>().Property(c => c.NEXT_ANN_REV_DATE).HasColumnName("NEXT_ANN_REV_DATE");
            */
        }
    }
}
