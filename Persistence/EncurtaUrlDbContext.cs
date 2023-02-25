using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EncurtaURL.Entities;
using Microsoft.EntityFrameworkCore;
using EncurtaURL;

namespace EncurtaURL.Persistence
{
    public class EncurtaUrlDbContext : DbContext
    {
        public EncurtaUrlDbContext(DbContextOptions<EncurtaUrlDbContext> options) : base(options)
        {
            
        }
        public DbSet<ShortenedCustomLink> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ShortenedCustomLink>(e =>
            {
                e.HasKey(l => l.Id);
            });
        }
    }
}