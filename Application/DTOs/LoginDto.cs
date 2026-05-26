using System.ComponentModel.DataAnnotations;

namespace FitnessTrackerPAW.Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}