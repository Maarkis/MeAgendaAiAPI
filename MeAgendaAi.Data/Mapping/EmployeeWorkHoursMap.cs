using MeAgendaAi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Mapping
{
    public class EmployeeWorkHoursMap : IEntityTypeConfiguration<EmployeeWorkHours>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkHours> builder)
        {
            builder.ToTable("EmployeeWorkHours");

            builder.HasKey(x => x.EmployeeWorkHoursId);

            builder.Property(x => x.StartHour)
                .IsRequired();

            builder.Property(x => x.EndHour)
               .IsRequired();

            builder.Property(x => x.StartInterval);

            builder.Property(x => x.EndInterval);

            builder.HasOne(x => x.Employee)
                .WithMany(y => y.EmployeeWorkHours)
                .HasForeignKey(x => x.EmployeeId);

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
