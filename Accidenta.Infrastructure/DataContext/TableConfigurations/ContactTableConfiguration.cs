using Accidenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Infrastructure.DataContext.TableConfigurations
{
    internal class ContactTableConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> entity)
        {
            entity.HasIndex(c => c.Email).IsUnique();

            entity
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Contact)
                .HasForeignKey(a => a.ContactId);
        }
    }
}
