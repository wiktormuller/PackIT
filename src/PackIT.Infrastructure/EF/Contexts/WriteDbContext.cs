using Microsoft.EntityFrameworkCore;
using PackIT.Domain.Entities;
using PackIT.Domain.ValueObjects;
using PackIT.Infrastructure.EF.Configurations;

namespace PackIT.Infrastructure.EF.Contexts
{
    internal class WriteDbContext : DbContext
    {
        public DbSet<PackingList> PackingLists { get; set; }

        public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("packing");

            var configuration = new WriteConfiguration();
            modelBuilder.ApplyConfiguration<PackingList>(configuration);
            modelBuilder.ApplyConfiguration<PackingItem>(configuration);

            base.OnModelCreating(modelBuilder);
        }
    }
}
