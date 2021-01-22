using MeAgendaAi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Mapping
{
    class LocationMap : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");

            builder.HasKey(x => x.LocationId);

            builder.HasOne(x => x.User)
                .WithMany(y => y.Locations)
                .HasForeignKey(x => x.UserId);

            builder.Property(x => x.Name);

            builder.Property(x => x.Country);

            builder.Property(x => x.State);

            builder.Property(x => x.City);

            builder.Property(x => x.Neighbourhood);

            builder.Property(x => x.Street);

            builder.Property(x => x.Number);

            builder.Property(x => x.Complement);

            builder.Property(x => x.CEP);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValue(new DateTime(2010, 1, 1));

            builder.Property(x => x.LastUpdatedAt)
                .IsRequired()
                .HasDefaultValue(new DateTime(2010, 1, 1));

            builder.Property(x => x.UpdatedBy)
                .IsRequired()
                .HasDefaultValue(new Guid());
        }
    }
}
