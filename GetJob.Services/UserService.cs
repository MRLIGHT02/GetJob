using GetJob.Data;
using GetJob.Entities;
using GetJob.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GetJob.Services
{
    public class UserService : IUserService
    {
        private readonly JobPortalContext _context;
        public UserService(JobPortalContext context)
        {
            _context = context;
        }

        #region AuthenticationANDRegistration
        // Authentication & Registration
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="user">The <see cref="User"/> entity containing user details (except password).</param>
        /// <param name="password">The plain text password which will be hashed before saving.</param>
        /// <returns>
        /// The created <see cref="User"/> object after being saved in the database.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when a user with the same email already exists.
        /// </exception>
        public async Task<User> RegisterAsync(User user, string password)
        {



            // Checking if email pehle se exist karta hai
            if(await _context.Users.AnyAsync(u=>u.Email == user.Email))
            {
                throw new Exception("User Already Exists with same Email");
            }
            // Password hash create karna
            user.PasswordHash = HashPassword(password);
            // Save User

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        /// <summary>
        /// Attempts to authenticate a user by email and password.
        /// </summary>
        /// <param name="email">
        /// The user's email address used to look up the account.
        /// </param>
        /// <param name="password">
        /// The plain-text password provided for verification. It will be hashed
        /// using <see cref="HashPassword(string)"/> and compared with the stored hash.
        /// </param>
        /// <returns>
        /// The <see cref="User"/> if the email exists and the password matches; otherwise <c>null</c>.
        /// </returns>
        /// <remarks>
        /// - Database lookup is asynchronous via EF Core (<c>FirstOrDefaultAsync</c>).<br/>
        /// - Returns <c>null</c> when user is not found or password is incorrect (no exception thrown).<br/>
        /// - Consider using a constant-time comparison to reduce timing attack surface,
        ///   and prefer salted, slow hashes (e.g., PBKDF2/Argon2) over simple hashing.
        /// </remarks>
        /// <example>
        /// <code>
        /// var user = await authService.LoginAsync("user@example.com", "P@ssw0rd!");
        /// if (user is null)
        /// {
        ///     // Invalid credentials
        /// }
        /// else
        /// {
        ///     // Authenticated
        /// }
        /// </code>
        /// </example>
        public async Task<User?> LoginAsync(string email, string password)
        {
            // Checking User Exist or Not
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email==email);

            if (user == null)
            {
                return null; // User Not found
            }

            // password hash compare 
            var hashPassword = HashPassword(password);

            if (user.PasswordHash !=hashPassword)
            {
                return null; // password incorrect

            }
            // if correct return the user
            return user;
        }
        #endregion

        #region MakingHash
        /// <summary>
        /// Hashes a plain-text password using SHA256 and returns a Base64-encoded string.
        /// </summary>
        /// <param name="password">
        /// The plain-text password to hash.
        /// </param>
        /// <returns>
        /// A Base64-encoded SHA256 hash of the input password.
        /// </returns>
        /// <remarks>
        /// ⚠️ Note: Using SHA256 directly for password hashing is insecure. This is Only for Testing. 
        /// Always prefer algorithms like PBKDF2, BCrypt, or Argon2 with salt for real applications.
        /// </remarks>
        private string? HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        #endregion

        #region CURDOPERATIONS
        // CRUD Operations
        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A collection of <see cref="User"/> entities.</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
           var User= await _context.Users.ToListAsync();

            return User;
        }
        /// <summary>
        /// Deletes a user from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

        }
        /// <summary>
        /// Retrieves a user by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The <see cref="User"/> if found; otherwise, null.</returns>
        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null) { 
                return user;
            
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user entity with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);

            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;
                

                // password update only if provided
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    existingUser.PasswordHash = user.PasswordHash;
                }

                await _context.SaveChangesAsync();
            }
        }

        #endregion

        #region PROFILEMANAGEMENT
            // Profile Management

            /// <summary>
            /// Changes the password for a given user if the old password is correct.
            /// </summary>
            /// <param name="id">The user ID.</param>
            /// <param name="oldPassword">The current (old) plain text password.</param>
            /// <param name="newPassword">The new plain text password to set.</param>
            /// <returns>True if password was successfully changed; otherwise, false.</returns>
        public async Task<bool> ChangePasswordAsync(int id, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return false;
            }
            // verify old password
            var oldHashed = HashPassword(oldPassword);
            if (user.PasswordHash !=oldHashed)
            {
                return false;
            }

            // set now new passowrd
            user.PasswordHash= HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates the profile information of an existing user.  
        /// This method is typically used when a user wants to update  
        /// their own details such as <c>Name</c>, <c>Email</c>, or other  
        /// non-sensitive profile information.  
        /// </summary>
        /// <param name="user">
        /// The <see cref="User"/> object containing updated profile details.  
        /// The <c>UserId</c> property must correspond to an existing user in the database.  
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation.  
        /// Returns <c>true</c> if the profile was successfully updated;  
        /// otherwise, <c>false</c>.  
        /// </returns>
        public async Task<bool> UpdateProfileAsync(User user)
        {
            var existing = await _context.Users.FindAsync(user.UserId);
            if (existing == null)
            { 
                return false;
            }

            existing.Name = user.Name;
            existing.Email = user.Email;
            

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region ROLEMANAGEMENT
        // Role Management

        /// <summary>
        /// Assigns or updates the role of an existing user.  
        /// This method is generally intended for administrative use,  
        /// allowing an admin to change a user's role (e.g., Candidate, Recruiter, Admin).  
        /// </summary>
        /// <param name="id">
        /// The unique identifier (<c>UserId</c>) of the user whose role is to be updated.  
        /// </param>
        /// <param name="role">
        /// The new role to assign to the user.  
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.  
        /// </returns>
        public async Task AssignRoleAsync(int id, string role)
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                throw new Exception($"User with Id {id} not found.");
            }
            user.Role = role;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all users who have the specified role.  
        /// </summary>
        /// <param name="role">
        /// The role to filter users by (e.g., "Admin", "Candidate", "Recruiter").  
        /// </param>
        /// <returns>
        /// A collection of <see cref="User"/> objects that match the given role.  
        /// </returns>
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _context.Users.Where(u=>u.Role ==role).ToListAsync();
        }
        #endregion

        #region ACCOUNTSTATUS
        // Account Status

        /// <summary>
        /// Deactivates a user account by setting <c>IsActive</c> to false.  
        /// This is a soft delete approach so that the user's data is preserved  
        /// in the database but the account is no longer active.  
        /// </summary>
        /// <param name="id">
        /// The unique identifier (<c>UserId</c>) of the user to deactivate.  
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation.  
        /// Returns <c>true</c> if the user was successfully deactivated;  
        /// otherwise, <c>false</c>.  
        /// </returns>
        public async Task<bool> DeactivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive= false;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Activates a user account by setting its <c>IsActive</c> property to <c>true</c>.
        /// </summary>
        /// <param name="id">The unique identifier of the user to activate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains 
        /// <c>true</c> if the user was found and activated successfully; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> ActivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            user.IsActive = true;
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region SECURITY
        // Security

        /// <summary>
        /// Resets the password of a user by hashing the new password 
        /// and updating it in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose password is to be reset.</param>
        /// <param name="newPassword">The new plain-text password to set.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains <c>true</c> if the password was successfully reset; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> ResetPasswordAsync(int id, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) { 
            
                return false;

            }
            // Hash the new password
            var hashedPassword = HashPassword(newPassword);
            user.PasswordHash = hashedPassword;

            await _context.SaveChangesAsync();
            return true;
        }
        public Task<bool> IsEmailTakenAsync(string email)
        {
            throw new NotImplementedException();
        }

        #endregion





    }
}
