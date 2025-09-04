using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetJob.Entites
{
    public class Jobs
    {
        public int JobId { get; set; }
        public string? Title { get; set; }            // Job Title (e.g. Software Engineer)
        public string? Description { get; set; }      // Full Job Description
        public string? Location { get; set; }         // Job Location
        public string? JobType { get; set; }          // Full-Time | Part-Time | Remote
        public decimal Salary { get; set; }          // Salary Offered
        public DateTime PostedDate { get; set; }     // When job was posted
        public DateTime ExpiryDate { get; set; }     // When job expires

        // Foreign Key
        public int EmployerId { get; set; }
        public User? Employer { get; set; }

        // Navigation
        public ICollection<Application>? Applications { get; set; }
    }
}
