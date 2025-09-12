using Microsoft.EntityFrameworkCore;
using training_catalog_api.Models;

namespace training_catalog_api.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Training> Trainings => Set<Training>();
        public DbSet<Category> Categories => Set<Category>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.CategoryName)
                      .IsRequired();
            });

            // Training
            modelBuilder.Entity<Training>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                      .HasMaxLength(120)
                      .IsRequired();

                entity.Property(t => t.ShortDescription)
                      .HasMaxLength(280)
                      .IsRequired();

                entity.Property(t => t.LongDescription)
                      .IsRequired();

                entity.HasOne(t => t.Category)
                      .WithMany(c => c.Trainings)
                      .HasForeignKey(t => t.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}