using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetJob.Entites
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public DateTime AppliedDate { get; set; }     // When candidate applied
        public string? Status { get; set; }            // Pending | Shortlisted | Rejected | Hired
        public string? CoverLetter { get; set; }       // Optional cover letter

        // Foreign Keys
        public int JobId { get; set; }
        public Jobs? Job { get; set; }

        public int CandidateId { get; set; }
        public User? Candidate { get; set; }
    }
}
