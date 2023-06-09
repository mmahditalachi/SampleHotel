using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleHotel.Models;

namespace SampleHotel.Infrastructure.EntityConfigurations
{
    public class TicketEntityConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable(nameof(Ticket));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Title)
              .HasMaxLength(75);

            builder.Property(e => e.Description)
              .HasMaxLength(250);

            builder.HasOne<ApplicationUser>()
              .WithMany()
              .HasForeignKey(e => e.Creator);
        }
    }

    public class TicketMessageEntityConfiguration : IEntityTypeConfiguration<TicketMessage>
    {
        public void Configure(EntityTypeBuilder<TicketMessage> builder)
        {
            builder.ToTable(nameof(TicketMessage));

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Title)
                .HasMaxLength(150);

            builder.HasOne(e => e.Ticket)
                .WithMany()
                .HasForeignKey(e => e.TicketId);

            builder.HasOne<ApplicationUser>()
              .WithMany()
              .HasForeignKey(e => e.Sender)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
