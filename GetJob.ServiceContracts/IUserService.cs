using GetJob.Entities;

namespace GetJob.ServiceContracts
{
    public interface IUserService
    {
        #region METHODS
        // 🔹 Authentication & Registration
        Task<User> RegisterAsync(User user, string password);
        Task<User?> LoginAsync(string email, string password);

        // 🔹 CRUD Operations
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

        // 🔹 Profile Management
        Task<bool> ChangePasswordAsync(int id, string oldPassword, string newPassword);
        Task<bool> UpdateProfileAsync(User user);

        // 🔹 Role Management
        Task AssignRoleAsync(int id, string role);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);

        // 🔹 Account Status
        Task<bool> DeactivateUserAsync(int id);
        Task<bool> ActivateUserAsync(int id);

        // 🔹 Security
        Task<bool> ResetPasswordAsync(int id, string newPassword);
        Task<bool> IsEmailTakenAsync(string email);
        #endregion
    }
}
