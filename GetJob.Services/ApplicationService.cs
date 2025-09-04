using GetJob.Data;
using GetJob.Entities;
using GetJob.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace GetJob.Services
{
    public class ApplicationService : IApplicationService
    {
        #region Dipendency
        private readonly JobPortalContext _context;
        #endregion

        #region Constructor
        public ApplicationService(JobPortalContext context)
        {
            _context = context;
        }
        #endregion

        #region METHODS

        /// <summary>
        /// Creates a new job application and saves it to the database.
        /// </summary>
        /// <param name="application">The application entity containing candidate and job details.</param>
        /// <returns>The application entity that was successfully saved.</returns>
        public async Task<Application> ApplyAsync(Application application)
        {
            application.AppliedDate = DateTime.Now;
            application.Status = ApplicationStatus.Pending.ToString();
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();

            return application;
        }

        /// <summary>
        /// Asynchronously deletes an Application entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the application to delete.</param>
        /// <returns>
        /// A task that represents the asynchronous delete operation. 
        /// The task result contains <c>true</c> if the application was successfully deleted; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var application = await _context.Applications.FindAsync(id);
                if (application == null)
                {
                    return false;
                }
                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Properly log the exception
                Console.WriteLine($"Error deleting application with ID {id}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves all applications asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task{IEnumerable{Application}}"/> representing the asynchronous operation,
        /// containing a collection of <see cref="Application"/> objects.</returns>
        public async Task<IEnumerable<Application>> GetAllAsync()
        {
            return await _context.Applications.ToListAsync();
        }

        /// <summary>
        /// Retrieves all applications for a specific candidate asynchronously.
        /// </summary>
        /// <param name="candidateId">The unique identifier of the candidate.</param>
        /// <returns>A <see cref="Task{IEnumerable{Application}}"/> containing the applications for the candidate.</returns>
        /// <exception cref="ArgumentNullException">Thrown when no applications are found for the given candidate.</exception>
        public async Task<IEnumerable<Application>> GetByCandidateIdAsync(int candidateId)
        {
            var currentCandidate = await _context.Applications.Where(candi => candi.CandidateId == candidateId).ToListAsync(); ;

            if (!currentCandidate.Any())
            {
                throw new KeyNotFoundException($"No applications found for candidate ID {candidateId}.");
            }

            return currentCandidate;
        }

        /// <summary>
        /// Retrieves an application by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the application.</param>
        /// <returns>
        /// A <see cref="Task{Application}"/> containing the application if found; otherwise, throws <see cref="KeyNotFoundException"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">Thrown when an application with the specified ID is not found.</exception>
        public async Task<Application?> GetByIdAsync(int id)
        {
            var current = await _context.Applications.FindAsync(id);
            if (current == null) throw new KeyNotFoundException("Id Not Found");
            return current;
        }
        /// <summary>
        /// Retrieves all applications for a specific job asynchronously.
        /// </summary>
        /// <param name="jobId">The unique identifier of the job.</param>
        /// <returns>A <see cref="Task{IEnumerable{Application}}"/> containing the applications for the job. Returns an empty list if none found.</returns>
        public async Task<IEnumerable<Application>> GetByJobIdAsync(int jobId)
        {
            return await _context.Applications.Where(job => job.JobId == jobId).ToListAsync();

        }

        /// <summary>
        /// Updates the status of an application asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the application.</param>
        /// <param name="status">The new status to set.</param>
        /// <returns>
        /// A <see cref="Task{bool}"/> indicating whether the update was successful.
        /// Returns <c>false</c> if the application with the specified ID does not exist.
        /// </returns>
        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return false;
            }
            application.Status = status;
            return true;
        }
        #endregion
    }
}
