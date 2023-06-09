using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleHotel.Models;

namespace SampleHotel.Infrastructure.EntityConfigurations
{
    public class RoomEntityConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable(nameof(Room));
            builder.HasKey(x => x.Id);

            builder.Property(e=>e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e=>e.ImageUrl)
                .HasMaxLength(150);

            builder.Property(e => e.Name)
                .HasMaxLength(75);

            builder.Property(e => e.Location)
                .HasMaxLength(75);

            builder.Property(e => e.ImageUrl)
                .HasMaxLength(150);

            builder.HasOne(e => e.RoomType)
                .WithMany()
                .HasForeignKey(e => e.RoomTypeId);
        }
    }

    public class RoomTypeEntityConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.ToTable(nameof(RoomType));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(150);            

            builder.HasData(Enumeration.GetAll<RoomType>());
        }
    }

    public class RoomReserveEntityConfiguration : IEntityTypeConfiguration<RoomReserve>
    {
        public void Configure(EntityTypeBuilder<RoomReserve> builder)
        {
            builder.ToTable(nameof(RoomReserve));
            builder.HasKey(x => x.Id);

            builder.HasOne(e => e.Room)
               .WithMany()
               .HasForeignKey(e => e.RoomId);

            builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(e => e.UserId);
        }
    }

    public class LikedRoomEntityConfiguration : IEntityTypeConfiguration<LikedRoom>
    {
        public void Configure(EntityTypeBuilder<LikedRoom> builder)
        {
            builder.ToTable(nameof(LikedRoom));
            builder.HasKey(x => new {x.UserId, x.RoomId });

            builder.HasOne<Room>()
               .WithMany()
               .HasForeignKey(e => e.RoomId);

            builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(e => e.UserId);
        }
    }
}
