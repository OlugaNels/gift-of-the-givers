using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GiftOfTheGivers.Website.Models;

namespace GiftOfTheGivers.Website.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Donation> Donations { get; set; }

        public DbSet<Volunteer> Volunteers { get; set; }

        public DbSet<ReliefProject> ReliefProjects { get; set; }

        public DbSet<ProjectUpdate> ProjectUpdates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Donation>()
                .Property(d => d.Amount)
                .HasPrecision(18, 2);

            builder.Entity<ReliefProject>()
                .Property(p => p.GoalAmount)
                .HasPrecision(18, 2);

            builder.Entity<ReliefProject>()
                .Property(p => p.RaisedAmount)
                .HasPrecision(18, 2);

            // A relief project has many progress updates. Deleting a project removes its
            // updates, since an update has no meaning independently of its project.
            builder.Entity<ProjectUpdate>()
                .HasOne(u => u.ReliefProject)
                .WithMany(p => p.Updates)
                .HasForeignKey(u => u.ReliefProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Updates are almost always read newest-first for a single project, so index
            // the foreign key together with the timestamp to support that access pattern.
            builder.Entity<ProjectUpdate>()
                .HasIndex(u => new { u.ReliefProjectId, u.PostedAt });
        }
    }
}
