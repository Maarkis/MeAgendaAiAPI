using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Mapping
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");

            builder.HasOne(x => x.User)
                .WithMany(y => y.Roles)
                .HasForeignKey(x => x.UserId);

            builder.Property(p => p.Role)
            .HasConversion(
                v => v.ToString(),
                v => (Roles)Enum.Parse(typeof(Roles), v));

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
