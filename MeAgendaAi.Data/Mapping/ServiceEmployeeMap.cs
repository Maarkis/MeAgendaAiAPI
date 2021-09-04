using System;
using MeAgendaAi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeAgendaAi.Data.Mapping
{
    public class ServiceEmployeeMap : IEntityTypeConfiguration<ServiceEmployee>
    {
        public void Configure(EntityTypeBuilder<ServiceEmployee> builder)
        {
            builder.ToTable("ServiceEmployee");

            builder.HasKey(x => x.EmployeeServiceId);

            builder.HasOne(x => x.Employee)
                .WithMany(e => e.EmployeeServices)
                .HasForeignKey(x => x.EmployeeId);

            builder.HasOne(x => x.Service)
                .WithMany(s => s.ServiceEmployees)
                .HasForeignKey(x => x.ServiceId);

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