using System;
using MeAgendaAi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeAgendaAi.Data.Mapping
{
    public class PhoneNumberMap : IEntityTypeConfiguration<PhoneNumber>
    {
        public void Configure(EntityTypeBuilder<PhoneNumber> builder)
        {
            builder.ToTable("PhoneNumber");

            builder.HasKey(x => x.PhoneNumberId);

            builder.HasOne(x => x.User)
                .WithMany(y => y.PhoneNumbers)
                .HasForeignKey(x => x.UserId);

            builder.Property(x => x.NameContact);

            builder.Property(x => x.CountryCode);

            builder.Property(x => x.DDD);

            builder.Property(x => x.Number).IsRequired();

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