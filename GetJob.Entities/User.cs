using System.ComponentModel.DataAnnotations;

namespace GetJob.Entities
{
    // The single user entity for both job seekers and employers.
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int? EmployerId { get; set; }
        public int? JobseekerId { get; set; }

        // Common properties
        public string? Name { get; set; }
        public string? Email { get; set; }

        // Store hashed password
        public string? PasswordHash { get; set; }

        // User role (Jobseeker / Employer)
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Jobseeker specific
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? ResumeUrl { get; set; }

        // ✅ Skills stored as comma-separated string
        // e.g. "C#, ASP.NET Core, SQL"
        public List<string>? Skills { get; set; }
        // Employer specific
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyWebsite { get; set; }

        // Navigation properties
        public ICollection<Job>? Jobs { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }

    // Enum for user roles
    public enum UserRole
    {
        Jobseeker,
        Employer,
        Admin
    }
}
