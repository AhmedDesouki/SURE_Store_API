using SURE_Store_API.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace SURE_Store_API.DTOs

{
    public class RegisterRequest
    {
        [Required]  // Validation: field is mandatory
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]  // Validation: 2-100 characters
        public string FullName { get; set; } = string.Empty;

        [Required]  // Validation: field is mandatory
        [EmailAddress(ErrorMessage = "Invalid email address")]  // Validation: must be valid email format
        public string Email { get; set; } = string.Empty;

        [Required]  // Validation: field is mandatory
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]  // Validation: minimum 6 characters
        [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,}$",   // Regex pattern for password complexity
            ErrorMessage = "Password must contain at least one number and one special character")]  // Custom error message
        public string Password { get; set; } = string.Empty;

        [Required]  // Validation: field is mandatory
        [Compare("Password", ErrorMessage = "Passwords do not match")]  // Validation: must match Password field
        public string ConfirmPassword { get; set; } = string.Empty;


    }
    public class LoginRequest {
        [Required]  // Validation: field is mandatory
        [EmailAddress(ErrorMessage = "Invalid email address")]  // Validation: must be valid email format
        public string Email { get; set; } = string.Empty;

        [Required]  // Validation: field is mandatory
        public string Password { get; set; } = string.Empty;
    }
    public class AuthResponse {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; } = null!;
        public List<string> Roles { get; set; } = new List<string>();
        public string Message { get; set; } = string.Empty;


    }
    public class UserDto {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    public class UpdateProfileRequest
    {
        [Required]  // Validation: field is mandatory
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]  // Validation: 2-100 characters
        public string FullName { get; set; } = string.Empty;
        [Required]  // Validation: field is mandatory
        [EmailAddress(ErrorMessage = "Invalid email address")]  // Validation: must be valid email format
        public string Email { get; set; } = string.Empty;

    }
    public class RefreshTokenRequest {

        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,}$",
           ErrorMessage = "Password must contain at least one number and one special character")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

 
}
