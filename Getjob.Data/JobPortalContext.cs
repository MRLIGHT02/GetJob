using Microsoft.EntityFrameworkCore;
using GetJob.Entities;

namespace GetJob.Data
{
    public class JobPortalContext : DbContext
    {
        public JobPortalContext(DbContextOptions<JobPortalContext> options) : base(options)
        {
        }

        // Only one DbSet is needed for the User entity
        public DbSet<User> Users { get; set; }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationships using the single User entity
            modelBuilder.Entity<User>()
                .HasMany(u => u.Jobs)
                .WithOne(j => j.Employer)
                .HasForeignKey(j => j.EmployerId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Applications)
                .WithOne(a => a.Jobseeker)
                .HasForeignKey(a => a.JobseekerId);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId);
        }
    }
}
