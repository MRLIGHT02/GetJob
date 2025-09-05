using GetJob.Entities;

namespace GetJob.Dtos
{
    public class UserRegistrationDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }

    public class UserLoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }


    public class UserPublicDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? ResumeUrl { get; set; }
        public List<string>? Skills { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyWebsite { get; set; }
    }


    public class UserProfileUpdateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? ResumeUrl { get; set; }
        public List<string>? Skills { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyWebsite { get; set; }
    }


    public class PasswordChangeDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    
    public class AssignRoleDto
    {
        public UserRole Role { get; set; }
    }


    public class ResetPasswordDto
    {
        public string NewPassword { get; set; }
    }

    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? ResumeUrl { get; set; }
        public List<string>? Skills { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyWebsite { get; set; }
    }
}
