using GetJob.Entities;

namespace GetJob.ServiceContracts.DTOs
{
    // Employer uploads job
    public class JobCreateDto
    {

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Salary { get; set; }

        // EmployerId must be set to know who created the job
       public int? EmployerId { get; set; }
    }

    // For updating job
    public class JobUpdateDto
    {
        public int JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }

    // For showing job details (read-only)
    public class JobResponseDto
    {
        public int JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime PostedDate { get; set; }

        public int EmployerId { get; set; }
        public string EmployerName { get; set; } = string.Empty;
    }
}
