using System.ComponentModel.DataAnnotations;

namespace GetJob.Entities
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        public DateTime AppliedDate { get; set; }
        public string? Status { get; set; }

        public string? ResumeUrl { get; set; }
        public string? CoverLetter { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Foreign Keys
        public int JobId { get; set; }
        public Job? Job { get; set; }

        // Now uses the generic User type for the relationship
        public int JobseekerId { get; set; }
        public User? Jobseeker { get; set; }
    }
}