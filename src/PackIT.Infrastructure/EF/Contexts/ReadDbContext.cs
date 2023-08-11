using Microsoft.EntityFrameworkCore;
using PackIT.Infrastructure.EF.Configurations;
using PackIT.Infrastructure.EF.Models;

namespace PackIT.Infrastructure.EF.Contexts
{
    internal class ReadDbContext : DbContext
    {
        public DbSet<PackingListReadModel> PackingLists { get; set; }

        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("packing");

            var configuration = new ReadConfiguration();
            modelBuilder.ApplyConfiguration<PackingListReadModel>(configuration);
            modelBuilder.ApplyConfiguration<PackingItemReadModel>(configuration);

            base.OnModelCreating(modelBuilder);
        }
    }
}
