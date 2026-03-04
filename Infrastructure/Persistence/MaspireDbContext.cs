using maspire_angular.Core.Models;
using maspire_angular.Features.Auth;
using maspire_angular.Features.Feature;
using maspire_angular.Features.Make;
using maspire_angular.Features.Photo;
using maspire_angular.Features.Vehicle;
using Microsoft.EntityFrameworkCore;

namespace maspire_angular.Infrastructure.Persistence
{
    public class MaspireDbContext : DbContext
    {
        public DbSet<Make> Makes { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureTranslation> FeatureTranslations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
        public MaspireDbContext(DbContextOptions<MaspireDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleFeature>()
                .HasKey(vf => new { vf.VehicleId, vf.FeatureId });

            // feature translation uses a unique index per feature/language and FK relationship
            modelBuilder.Entity<FeatureTranslation>(entity =>
            {
                entity.HasIndex(ft => new { ft.FeatureId, ft.Language }).IsUnique();

                entity.HasOne(ft => ft.Feature)
                      .WithMany(f => f.Translations)
                      .HasForeignKey(ft => ft.FeatureId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}