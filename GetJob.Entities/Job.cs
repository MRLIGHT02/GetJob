using System.ComponentModel.DataAnnotations;

namespace GetJob.Entities
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public decimal Salary { get; set; }
        public DateTime PostedDate { get; set; }

        // Now uses the generic User type for the relationship
        public int EmployerId { get; set; }
        public User? Employer { get; set; }

        // Navigation for applications
        public ICollection<Application>? Applications { get; set; }
    }
}
