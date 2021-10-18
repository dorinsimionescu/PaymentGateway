using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PaymentGateway.Models;

#nullable disable

namespace PaymentGateway.Data
{
    public partial class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductXtransaction> ProductXtransactions { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Limit).HasColumnType("money");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasIndex(e => e.Cnp, "IX_Person")
                    .IsUnique();

                entity.Property(e => e.Cnp)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Limit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Value).HasColumnType("money");
            });

            modelBuilder.Entity<ProductXtransaction>(entity =>
            {
                entity.HasKey(e => new { e.IdTransaction, e.IdProduct });

                entity.ToTable("ProductXTransaction");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductXtransactions)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductXTransaction_Product");

                entity.HasOne(d => d.IdTransactionNavigation)
                    .WithMany(p => p.ProductXtransactions)
                    .HasForeignKey(d => d.IdTransaction)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductXTransaction_Transaction");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_BankAccounts");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
