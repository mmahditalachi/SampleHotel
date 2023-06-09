using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleHotel.Models;

namespace SampleHotel.Infrastructure.EntityConfigurations
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(e => e.FullName)
                .HasMaxLength(75)
                .IsRequired();

            builder.Property(e => e.Country)
                .HasMaxLength(75)
                .IsRequired();

            builder.Property(e => e.City)
                .HasMaxLength(75)
                .IsRequired();

            builder.Property(e => e.No)
                .HasMaxLength(75)
                .IsRequired();
        }
    }
}
