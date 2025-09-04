using GetJob.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetJob.ServiceContracts.DTOs
{
    public record UserRegistrationDto(string Name, string Email, string Password, string Role);
    public record UserLoginDto(string Email, string Password);
    public record UserProfileUpdateDto(string Name, string Email, string PhoneNumber, string Location, string ProfilePictureUrl, string ResumeUrl, List<string>? Skills);
    public record PasswordChangeDto(string OldPassword, string NewPassword);
    public record UserPublicDto(int UserId, string Name, string Email, string Role, string? PhoneNumber, string? Location, List<string>? Skills, bool IsActive);
    public record AssignRoleDto(string Role);
    public record ResetPasswordDto(string NewPassword);
}
