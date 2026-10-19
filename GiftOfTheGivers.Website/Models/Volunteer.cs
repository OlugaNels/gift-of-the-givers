using System;

namespace GiftOfTheGivers.Website.Models
{
    public class Volunteer
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string Skills { get; set; } = string.Empty;

        public string Availability { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTime JoinedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
