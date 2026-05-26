using System.ComponentModel.DataAnnotations;

namespace FitnessTrackerPAW.Application.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Format de email invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [MinLength(6, ErrorMessage = "Parola trebuie sa aiba minim 6 caractere.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}