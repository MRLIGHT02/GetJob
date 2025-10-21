using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetJob.ServiceContracts.DTOs
{
    public class ApplicationCreateDto
    {
        public int JobId { get; set; }
        public int JobseekerId { get; set; }
        public string? ResumeUrl { get; set; }
        public string? CoverLetter { get; set; }
    }

    // 🔹 Update an existing Application
    public class ApplicationUpdateDto
    {
        public string? ResumeUrl { get; set; }
        public string? CoverLetter { get; set; }
        public string? Status { get; set; }
    }

    // 🔹 Update status only
    public class ApplicationStatusUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }

    // 🔹 Read / Response DTO
    public class ApplicationResponseDto
    {
        public int ApplicationId { get; set; }
        public DateTime AppliedDate { get; set; }
        public string? Status { get; set; }
        public string? ResumeUrl { get; set; }
        public string? CoverLetter { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Related info
        public int JobId { get; set; }
        public string? JobTitle { get; set; }

        public int JobseekerId { get; set; }
        public string? JobseekerName { get; set; }
    }

}
