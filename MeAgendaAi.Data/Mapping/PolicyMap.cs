using System;
using MeAgendaAi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeAgendaAi.Data.Mapping
{
    public class PolicyMap : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.ToTable("Policy");

            builder.HasKey(x => x.PolicyId);

            builder.HasOne(p => p.Company)
                .WithOne(c => c.Policy)
                .HasForeignKey<Policy>(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.LimitCancelHours).HasDefaultValue(1);

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