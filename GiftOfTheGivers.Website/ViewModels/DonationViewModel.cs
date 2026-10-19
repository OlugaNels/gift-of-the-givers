using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Website.ViewModels
{
    public class DonationViewModel
    {
        [Required(ErrorMessage = "Please enter an amount.")]
        [Range(1, 1000000, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please select a currency.")]
        public string Currency { get; set; } = "ZAR";

        public bool IsRecurring { get; set; }

        public bool IsAnonymous { get; set; }

        // Only used if not anonymous
        public string? DonorName { get; set; }
        public string? DonorEmail { get; set; }
    }
}
