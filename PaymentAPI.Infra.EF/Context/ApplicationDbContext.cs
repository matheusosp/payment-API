using Microsoft.EntityFrameworkCore;
using PaymentAPI.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Infra.EF.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Item> Items { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sale>()
                .HasMany(i => i.Items);

        }
    }
}
