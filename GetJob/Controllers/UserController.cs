using GetJob.Entities;
using GetJob.ServiceContracts;
using GetJob.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GetJob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        #region Box And Item
        private readonly IUserService _userService;

        #region Constructor
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Authentication & Registration

        /// <summary>
        /// Registers a new user using the provided registration DTO.
        /// Maps the DTO to the User entity and returns a public DTO.
        /// </summary>
        /// <param name="registrationDto">The registration details including name, email, password, and role.</param>
        /// <returns>Returns the created user as a public DTO.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            try
            {
                // Map the DTO to the entity
                var newUser = new User
                {
                    Name = registrationDto.Name,
                    Email = registrationDto.Email,
                    Role = (UserRole)Enum.Parse(typeof(UserRole), registrationDto.Role!)
                };

                var createdUser = await _userService.RegisterAsync(newUser, registrationDto.Password!);

                // Map the created entity to the correct DTO for the response
                var publicUser = new UserProfileDto
                {
                    UserId = createdUser.UserId,
                    Name = createdUser.Name,
                    Email = createdUser.Email,
                    Role = createdUser.Role,
                    PhoneNumber = createdUser.PhoneNumber,
                    Location = createdUser.Location,
                    ProfilePictureUrl = createdUser.ProfilePictureUrl,
                    ResumeUrl = createdUser.ResumeUrl,
                    Skills = createdUser.Skills,
                
                    CompanyName = createdUser.CompanyName,
                    CompanyDescription = createdUser.CompanyDescription,
                    CompanyWebsite = createdUser.CompanyWebsite
                };
             
                return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, publicUser);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Authenticates a user using email and password.
        /// Returns a public DTO if login is successful.
        /// </summary>
        /// <param name="loginDto">The login details including email and password.</param>
        /// <returns>Returns the authenticated user as a public DTO or Unauthorized if credentials are invalid.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto.Email!, loginDto.Password!);
            if (user == null) return Unauthorized("Invalid credentials");

            // Return a DTO to avoid exposing the password hash
            var publicUser = new UserProfileDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                
                PhoneNumber = user.PhoneNumber,
                Location = user.Location,

                ProfilePictureUrl = user.ProfilePictureUrl,
                ResumeUrl = user.ResumeUrl,
                Skills = user.Skills,
                CompanyName = user.CompanyName,
                CompanyDescription = user.CompanyDescription,
                CompanyWebsite = user.CompanyWebsite

            };
        

            return Ok(publicUser);
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Retrieves all users in the system.
        /// Maps entities to public DTOs to avoid exposing sensitive data.
        /// </summary>
        /// <returns>A list of public user DTOs.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            // Map all entities to DTOs before returning
            var publicUsers = users.Select(u => new UserProfileDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                PhoneNumber = u.PhoneNumber,
                Location = u.Location,
                ProfilePictureUrl = u.ProfilePictureUrl,
                ResumeUrl = u.ResumeUrl,
                Skills = u.Skills,
                CompanyName = u.CompanyName,
                CompanyDescription = u.CompanyDescription,
                CompanyWebsite = u.CompanyWebsite
            });
            return Ok(publicUsers);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The public DTO of the user if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            // Return a DTO to avoid exposing sensitive data
            var publicUser = new UserProfileDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                Location = user.Location,
                ProfilePictureUrl = user.ProfilePictureUrl,
                ResumeUrl = user.ResumeUrl,
                Skills = user.Skills,
                CompanyName = user.CompanyName,
                CompanyDescription = user.CompanyDescription,
                CompanyWebsite = user.CompanyWebsite
            };
            return Ok(publicUser);
        }

        /// <summary>
        /// Updates a user's profile using the provided DTO.
        /// Only allowed properties are updated.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="userDto">The DTO containing updated profile information.</param>
        /// <returns>NoContent if successful, NotFound if the user does not exist.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserProfileUpdateDto userDto)
        {
            var existingUser = await _userService.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            // Map DTO to entity and update only allowed properties
            existingUser.Name = userDto.Name;
            existingUser.Email = userDto.Email;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            existingUser.Location = userDto.Location;
            existingUser.ProfilePictureUrl = userDto.ProfilePictureUrl;
            existingUser.ResumeUrl = userDto.ResumeUrl;
            existingUser.Skills = userDto.Skills;
            existingUser.CompanyName = userDto.CompanyName;
            existingUser.CompanyDescription = userDto.CompanyDescription;
            existingUser.CompanyWebsite = userDto.CompanyWebsite;
           
            var success = await _userService.UpdateProfileAsync(existingUser);
            if (!success) return BadRequest("Failed to update profile.");

            return NoContent();
        }

        /// <summary>
        /// Deletes a user from the system by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>NoContent after deletion.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        #endregion

        #region Profile Management

        /// <summary>
        /// Changes the password of a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="passwords">DTO containing old and new passwords.</param>
        /// <returns>NoContent if successful, BadRequest if the old password is incorrect or user not found.</returns>
        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] PasswordChangeDto passwords)
        {
            var success = await _userService.ChangePasswordAsync(id, passwords.OldPassword, passwords.NewPassword);
            if (!success) return BadRequest("Old password is incorrect or user not found");
            return NoContent();
        }

        #endregion

        #region Role Management

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="roleDto">DTO containing the role to assign.</param>
        /// <returns>NoContent if successful, NotFound if the user does not exist.</returns>
        [HttpPut("{id}/assign-role")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] AssignRoleDto roleDto)
        {
            try
            {
                await _userService.AssignRoleAsync(id, roleDto.Role);
                return NoContent(); // 204 success, as no content is returned
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // 404 Not Found
            }
        }

        /// <summary>
        /// Retrieves all users with a specific role.
        /// </summary>
        /// <param name="role">The role to filter users by.</param>
        /// <returns>A list of users with the specified role.</returns>
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetUsersByRole(UserRole role)
        {
            var users = await _userService.GetUsersByRoleAsync(role);
            var publicUsers = users.Select(u => new UserProfileDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                PhoneNumber = u.PhoneNumber,
                Location = u.Location,
                ProfilePictureUrl = u.ProfilePictureUrl,
                ResumeUrl = u.ResumeUrl,
                Skills = u.Skills,
                CompanyName = u.CompanyName,
                CompanyDescription = u.CompanyDescription,
                CompanyWebsite = u.CompanyWebsite
            });
            return Ok(publicUsers);
        }

        #endregion

        #region Account Status
        /// <summary>
        /// Deactivates a user account.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>NoContent if successful, NotFound if the user does not exist.</returns>
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var success = await _userService.DeactivateUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        /// <summary>
        /// Activates a user account.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>NoContent if successful, NotFound if the user does not exist.</returns>
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var success = await _userService.ActivateUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        #endregion

        #region Security
        /// <summary>
        /// Resets a user's password.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="resetDto">DTO containing the new password.</param>
        /// <returns>NoContent if successful, NotFound if the user does not exist.</returns>
        [HttpPut("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordDto resetDto)
        {
            var success = await _userService.ResetPasswordAsync(id, resetDto.NewPassword);

            // A more descriptive response for failure
            if (!success)
            {
                return NotFound("User not found.");
            }

            return NoContent();
        }
        /// <summary>
        /// Checks if an email is already taken by any user.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if email is taken, otherwise false.</returns>
        [HttpGet("is-email-taken")]
        public async Task<IActionResult> IsEmailTaken([FromQuery] string email)
        {
            var result = await _userService.IsEmailTakenAsync(email);
            return Ok(result);
        }

        #endregion

#endregion
    }
}