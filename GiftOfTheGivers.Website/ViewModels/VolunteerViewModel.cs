using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Website.ViewModels
{
    public class VolunteerViewModel
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please tell us your skills.")]
        public string Skills { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please tell us your availability.")]
        public string Availability { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
