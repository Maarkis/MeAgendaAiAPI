using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Mapping
{
    public class EmployeeServiceMap : IEntityTypeConfiguration<EmployeeService>
    {
        public void Configure(EntityTypeBuilder<EmployeeService> builder)
        {
            builder.ToTable("EmployeeService");

            builder.HasKey(x => x.EmployeeServiceId);

            builder.HasOne(x => x.Employee)
                .WithMany(e => e.EmployeeServices)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Service)
                .WithMany(s => s.ServiceEmployees)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValue(DateTimeUtil.UtcToBrasilia());

            builder.Property(x => x.LastUpdatedAt)
                .IsRequired()
                .HasDefaultValue(DateTimeUtil.UtcToBrasilia());

            builder.Property(x => x.UpdatedBy)
                .IsRequired()
                .HasDefaultValue(new Guid());
        }
    }
}
