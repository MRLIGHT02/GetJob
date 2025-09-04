using GetJob.Entities;
using GetJob.ServiceContracts;
using GetJob.ServiceContracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GetJob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        #region Authentication & Registration

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
                    Role = registrationDto.Role
                };

                var createdUser = await _userService.RegisterAsync(newUser, registrationDto.Password);

                // Map the created entity back to a public DTO for the response
                var publicUser = new UserPublicDto(createdUser.UserId, createdUser.Name!, createdUser.Email!, createdUser.Role!, createdUser.PhoneNumber, createdUser.Location, createdUser.Skills, createdUser.IsActive);

                return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, publicUser);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto.Email, loginDto.Password);
            if (user == null) return Unauthorized("Invalid credentials");

            // Return a DTO to avoid exposing the password hash
            var publicUser = new UserPublicDto(user.UserId, user.Name!, user.Email!, user.Role!, user.PhoneNumber, user.Location, user.Skills, user.IsActive);

            return Ok(publicUser);
        }

        #endregion

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            // Map all entities to DTOs before returning
            var publicUsers = users.Select(u => new UserPublicDto(u.UserId, u.Name!, u.Email!, u.Role!, u.PhoneNumber, u.Location, u.Skills, u.IsActive));
            return Ok(publicUsers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            // Return a DTO to avoid exposing sensitive data
            var publicUser = new UserPublicDto(user.UserId, user.Name!, user.Email!, user.Role!, user.PhoneNumber, user.Location, user.Skills, user.IsActive);

            return Ok(publicUser);
        }

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

            var success = await _userService.UpdateProfileAsync(existingUser);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        #endregion

        #region Profile Management

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] PasswordChangeDto passwords)
        {
            var success = await _userService.ChangePasswordAsync(id, passwords.OldPassword, passwords.NewPassword);
            if (!success) return BadRequest("Old password is incorrect or user not found");
            return NoContent();
        }

        [HttpPut("{id}/update-profile")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UserProfileUpdateDto userDto)
        {
            // 1. Fetch the existing user entity from the database
            var existingUser = await _userService.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // 2. Manually map the allowed properties from the DTO to the entity
            existingUser.Name = userDto.Name;
            existingUser.Email = userDto.Email;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            existingUser.Location = userDto.Location;
            existingUser.ProfilePictureUrl = userDto.ProfilePictureUrl;
            existingUser.ResumeUrl = userDto.ResumeUrl;
            existingUser.Skills = userDto.Skills;

            // 3. Pass the *entity* to the service for updating
            var success = await _userService.UpdateProfileAsync(existingUser);
            if (!success)
            {
                return BadRequest("Failed to update profile.");
            }

            return NoContent();
        }

        #endregion

        #region Role Management


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
        [HttpGet("role/{role}")]
        public async Task<IEnumerable<User>> GetUsersByRole(string role)
        {
            return await _userService.GetUsersByRoleAsync(role);
        }

        #endregion

        #region Account Status

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var success = await _userService.DeactivateUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var success = await _userService.ActivateUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        #endregion

        #region Security

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

        [HttpGet("is-email-taken")]
        public async Task<IActionResult> IsEmailTaken([FromQuery] string email)
        {
            var result = await _userService.IsEmailTakenAsync(email);
            return Ok(result);
        }

        #endregion
    }
}
