using System;
using MeAgendaAi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeAgendaAi.Data.Mapping
{
    public class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");

            builder.HasKey(x => x.ClientId);

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.CPF)
                .IsRequired();

            builder.Property(x => x.RG)
                .IsRequired();

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