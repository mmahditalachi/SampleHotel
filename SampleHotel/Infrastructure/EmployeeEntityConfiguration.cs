using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleHotel.Models;

namespace SampleHotel.Infrastructure
{
    public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ImageUrl)
                .HasMaxLength(150);

            builder.Property(e => e.FirstName)
                .HasMaxLength(75);

            builder.Property(e => e.LastName)
                .HasMaxLength(75);

            builder.Property(e => e.ImageUrl)
                .HasMaxLength(150);

            builder.Property(e => e.City)
                .HasMaxLength(75);

            builder.Property(e => e.DependentFirstName)
                .HasMaxLength(75);

            builder.Property(e => e.DependentLastName)
                .HasMaxLength(75);

            builder.Property(e => e.DependentRelation)
                .HasMaxLength(75);

            builder.HasOne(e => e.EmployeePosition)
                .WithMany()
                .HasForeignKey(e => e.EmployeePositionId);
        }
    }

    public class EmployeePositionEntityConfiguration : IEntityTypeConfiguration<EmployeePosition>
    {
        public void Configure(EntityTypeBuilder<EmployeePosition> builder)
        {
            builder.ToTable(nameof(EmployeePosition));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(150);

            builder.HasData(Enumeration.GetAll<EmployeePosition>());
        }
    }
}
