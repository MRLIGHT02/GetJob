using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

        // Employer (Job posted by)
        public int EmployerId { get; set; }
        public User? Employer { get; set; }

        // Navigation
        public ICollection<Application>? Applications { get; set; }
    }
}
