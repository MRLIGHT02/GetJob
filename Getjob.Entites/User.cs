using System.ComponentModel.DataAnnotations;

namespace GetJob.Entites
{
    public class User
    {
        [Key]
        public int UserId { get; set; }                  // Primary Key
        public string? FullName { get; set; }         // Candidate/Employer Name
        public string? Email { get; set; }            // Login Email
        public string? PasswordHash { get; set; }     // Secure password storage
        public string? Phone { get; set; }            // Contact Number
        public string? Role { get; set; }             // "Candidate" | "Employer" | "Admin"
        public DateTime CreatedAt { get; set; }      // Account creation date
        public bool IsActive { get; set; }           // Active/Inactive account

        public string? Skills { get; set; }           // Candidate Skills (comma separated)
        public string? ResumePath { get; set; }       // File path for uploaded resume

       
        public string? CompanyName { get; set; }      // Employer Company
        public string? CompanyWebsite { get; set; }   // Employer Website

        // Navigation Properties
        public ICollection<Jobs>? Job { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }
}
