using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.EntityTypeConfiguration;
using PaymentGateway.Models;

namespace PaymentGateway.Data
{
    public class PaymentDbContext : DbContext
    {
        /*
         * DbContextOptions<PaymentDbContext> and must pass it to the base constructor for DbContext.'
         */
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {

        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ProductXTransaction> ProductXTransaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().Property(x => x.Id);//.HasColumnName("IdUlMeuSpecial");

            modelBuilder.Entity<ProductXTransaction>().HasKey(x => new { x.IdProduct, x.IdTransaction });

            modelBuilder.ApplyConfiguration(new PersonConfiguration());
        }
    }
}
