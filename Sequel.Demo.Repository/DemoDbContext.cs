using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sequel.Demo.Business.Models;
using Sequel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sequel.Demo.Repository
{
    public class DemoDbContext : BaseDbContext
    {
        public DemoDbContext(string connectionString)
            : base(connectionString, typeof(ThingMapping), typeof(OtherStuffMapping))
        {
        }

        public DbSet<Thing> Things { get; set; }
        public DbSet<OtherStuff> OtherStuffs { get; set; }
    }

    public class ThingMapping : IEntityTypeConfiguration<Thing>
    {
        public void Configure(EntityTypeBuilder<Thing> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasMany<OtherStuff>(o => o.OtherStuffs)
                .WithOne()
                .HasForeignKey(f => f.ThingId);
        }
    }

    public class OtherStuffMapping : IEntityTypeConfiguration<OtherStuff>
    {
        public void Configure(EntityTypeBuilder<OtherStuff> builder)
        {
            builder.HasKey(o => o.Id);
        }
    }
}
