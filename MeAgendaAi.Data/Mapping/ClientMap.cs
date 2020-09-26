using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Mapping
{
    public class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");

            builder.HasKey(x => x.ClientId);

            builder.Property(x => x.UserId).IsRequired();

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
