using System.ComponentModel.DataAnnotations;
namespace SURE_Store_API.DTOs.Auth
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Current password is required")]  // Validation attribute: field is mandatory
        [StringLength(100, ErrorMessage = "Current password cannot exceed 100 characters")]  // Validation attribute: maximum 100 characters
        public string CurrentPassword { get; set; } = string.Empty;  // Initialize as empty string to avoid null reference exceptions
        [Required(ErrorMessage = "New password is required")]  // Validation attribute: field is mandatory
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be at least 6 characters long")]  // Validation attribute: 6-100 characters
        public string NewPassword { get; set; } = string.Empty;  // Initialize as empty string to avoid null reference exceptions
        [Required(ErrorMessage = "Password confirmation is required")]  // Validation attribute: field is mandatory
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]  // Validation attribute: must match NewPassword property
        public string ConfirmNewPassword { get; set; } = string.Empty;  // Initialize as empty string to avoid null reference exceptions
    }
}
