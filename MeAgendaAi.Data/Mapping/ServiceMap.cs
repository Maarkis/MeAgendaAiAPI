using System;
using MeAgendaAi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeAgendaAi.Data.Mapping
{
    public class ServiceMap : IEntityTypeConfiguration<Services>
    {
        public void Configure(EntityTypeBuilder<Services> builder)
        {
            builder.ToTable("Service");

            builder.HasKey(x => x.ServiceId);

            builder.HasOne(x => x.Company)
                .WithMany(y => y.Services)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.DurationMinutes).HasDefaultValue(30);

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