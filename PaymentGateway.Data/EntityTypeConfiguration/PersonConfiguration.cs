using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGateway.Models;

namespace PaymentGateway.Data.EntityTypeConfiguration
{
    class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(u => new { u.Id });
            builder.Property(x => x.Name).HasMaxLength(255);
        }
    }
}