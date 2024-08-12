﻿using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AssetManagementProject.web.Entity.Context
{
    public partial class DefaultDbContext : DbContext
    {

        public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
	    public DbSet<WorkflowTe> WorkflowTes { get; set; }
	    public DbSet<WorkflowVersion> WorkflowVersions { get; set; }
        //lazy-loading
        //https://entityframeworkcore.com/querying-data-loading-eager-lazy
        //https://docs.microsoft.com/en-us/ef/core/querying/related-data
        //EF Core will enable lazy-loading for any navigation property that is virtual and in an entity class that can be inherited
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
        .UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent API
            modelBuilder.Entity<User>()
           .HasOne(u => u.Account)
           .WithMany(e => e.Users);

            //concurrency
            modelBuilder.Entity<Account>()
            .Property(a => a.RowVersion).IsRowVersion();
            modelBuilder.Entity<User>()
            .Property(a => a.RowVersion).IsRowVersion();
            modelBuilder.Entity<DynamicFormsConfiguration>()
            .Property(a => a.RowVersion).IsRowVersion();
            modelBuilder.Entity<WorkflowTe>()
            .Property(a => a.RowVersion).IsRowVersion();
            modelBuilder.Entity<WorkflowVersion>()
            .Property(a => a.RowVersion).IsRowVersion();

            SetAdditionalConcurency(modelBuilder);

            //modelBuilder.Entity<User>()
            //.Property(p => p.DecryptedPassword)
            //.HasComputedColumnSql("Uncrypt(p.PasswordText)");
        }

        //call scaffolded class method to add concurrency
        partial void SetAdditionalConcurency(ModelBuilder modelBuilder);

        public override int SaveChanges()
        {
            Audit();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            Audit();
            return await base.SaveChangesAsync();
        }

        private void Audit()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
                }
            ((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }

    }
}
