using Accidenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accidenta.Infrastructure.DataContext.TableConfigurations;

internal class IncidentTableConfiguration : IEntityTypeConfiguration<Incident>
{
    public void Configure(EntityTypeBuilder<Incident> entity)
    {
        entity.HasKey(i => i.Id);
    }
}
