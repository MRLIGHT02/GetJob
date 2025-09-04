using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetJob.Entities
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        public DateTime AppliedDate { get; set; }
        public string? Status { get; set; } // Pending / Accepted / Rejected

        // Foreign Keys
        public int JobId { get; set; }
        public Job? Job { get; set; }

        public int CandidateId { get; set; }
        public User? Candidate { get; set; }
    }
}
