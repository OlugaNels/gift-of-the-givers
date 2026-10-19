using System;

namespace GiftOfTheGivers.Website.Models
{
    public class Donation
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "ZAR";

        public bool IsRecurring { get; set; }

        public bool IsAnonymous { get; set; }

        public string? DonorName { get; set; }

        public string? DonorEmail { get; set; }

        public DateTime DonationDate { get; set; }
    }
}
