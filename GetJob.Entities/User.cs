using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace GetJob.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        // Basic Info
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }

        // Extra Profile Info
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? ResumeUrl { get; set; }
        public List<string>? Skills { get; set; } // e.g. "C#, SQL, Angular"

        // Account Management
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        // Relationships (agar baad me chahiye)
        public ICollection<Application>? Applications { get; set; }
        public ICollection<Job>? Jobs { get; set; }
    }
}
