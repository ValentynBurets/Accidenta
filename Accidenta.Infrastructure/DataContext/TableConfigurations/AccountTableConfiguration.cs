using Accidenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accidenta.Infrastructure.DataContext.TableConfigurations;

internal class AccountTableConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> entity)
    {
        entity.HasIndex(a => a.Name).IsUnique();

        entity
            .HasMany(a => a.Incidents)
            .WithOne(i => i.Account)
            .HasForeignKey(i => i.AccountId);
    }
}
