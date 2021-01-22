using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.UserId);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(60);

            builder.Property(x => x.Password);

            builder.Property(x => x.Image);

            //Roles
            //builder.Property(x => x.Roles)
            //    .IsRequired();                

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
