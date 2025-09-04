using GetJob.Entities;
using Microsoft.EntityFrameworkCore;

namespace GetJob.Data
{
    public class JobPortalContext : DbContext
    {
        public JobPortalContext(DbContextOptions<JobPortalContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Application -> User relation (Candidate)
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Candidate) // navigation property in Application
                .WithMany(u => u.Applications) // collection in User
                .HasForeignKey(a => a.CandidateId)
                .OnDelete(DeleteBehavior.Restrict); // 🚀 prevent cascade loop

            // Application -> Job relation
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict); // 🚀 prevent cascade loop
        }
    }
}
