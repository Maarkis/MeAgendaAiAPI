using System;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeAgendaAi.Data.Mapping
{
    public class SchedulingMap : IEntityTypeConfiguration<Scheduling>
    {
        public void Configure(EntityTypeBuilder<Scheduling> builder)
        {
            builder.ToTable("Scheduling");

            builder.HasKey(x => x.SchedulingId);

            builder.HasOne(s => s.Client)
                .WithMany(c => c.Schedulings)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Employee)
                .WithMany(c => c.Schedulings)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.ServiceId).IsRequired();

            builder.Property(x => x.StartTime).IsRequired();

            builder.Property(x => x.EndTime);

            builder.Property(x => x.Status)
                .HasDefaultValue(SchedulingStatus.Scheduled)
                .HasConversion(
                    x => x.ToString(),
                    x => (SchedulingStatus)Enum.Parse(typeof(SchedulingStatus), x));

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