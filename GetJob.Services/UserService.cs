using GetJob.Data;
using GetJob.Entities;
using GetJob.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GetJob.Services
{
    // A single, centralized service that implements the IUserService interface.
    public class UserService : IUserService
    {
        private readonly JobPortalContext _context;

        public UserService(JobPortalContext context)
        {
            _context = context;
        }

        // --- Authentication & Registration ---

        /// <summary>
        /// Registers a new user with a specific role.
        /// </summary>
        public async Task<User> RegisterAsync(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new Exception("A user with this email already exists.");
            }

            user.PasswordHash = HashPassword(password);
            user.IsActive = true;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Attempts to authenticate a user by email and password.
        /// </summary>
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.IsActive == false) return null;

            var hashedPassword = HashPassword(password);
            if (user.PasswordHash != hashedPassword)
            {
                return null;
            }

            return user;
        }

        // --- CRUD Operations ---

        /// <summary>
        /// Retrieves a user by their unique ID.
        /// </summary>
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Updates a user's profile information.
        /// </summary>
        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            existingUser.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a user from the database by ID.
        /// </summary>
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // --- Profile Management ---

        /// <summary>
        /// Changes a user's password after verifying the old password.
        /// </summary>
        public async Task<bool> ChangePasswordAsync(int id, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            if (user.PasswordHash != HashPassword(oldPassword)) return false;

            user.PasswordHash = HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates a user's profile details.
        /// </summary>
        public async Task<bool> UpdateProfileAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null) return false;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Location = user.Location;
            existingUser.ResumeUrl = user.ResumeUrl;
            existingUser.ProfilePictureUrl = user.ProfilePictureUrl;
            existingUser.CompanyName = user.CompanyName;
            existingUser.CompanyDescription = user.CompanyDescription;
            existingUser.CompanyWebsite = user.CompanyWebsite;
            existingUser.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // --- Role Management ---

        /// <summary>
        /// Assigns a new role to a user.
        /// </summary>
        public async Task AssignRoleAsync(int id, UserRole role)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            user.Role = role;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all users with a specific role.
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users.Where(u => u.Role == role).ToListAsync();
        }

        // --- Account Status ---

        /// <summary>
        /// Deactivates a user's account by setting IsActive to false.
        /// </summary>
        public async Task<bool> DeactivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Activates a user's account by setting IsActive to true.
        /// </summary>
        public async Task<bool> ActivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        // --- Security ---

        /// <summary>
        /// Resets a user's password without needing the old one.
        /// </summary>
        public async Task<bool> ResetPasswordAsync(int id, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.PasswordHash = HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Checks if an email is already taken by an existing user.
        /// </summary>
        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        // --- Private Helper Method ---

        /// <summary>
        /// Hashes a plain-text password using SHA256.
        /// </summary>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
