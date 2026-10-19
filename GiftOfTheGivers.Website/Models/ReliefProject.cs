using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Website.Models
{
    public class ReliefProject
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        /// <summary>Planned, Active or Completed — matches the Section B schema design.</summary>
        [MaxLength(20)]
        public string Status { get; set; } = "Planned";

        public decimal GoalAmount { get; set; }

        public decimal RaisedAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        // Progress updates posted against this project by Foundation employees.
        public ICollection<ProjectUpdate> Updates { get; set; } = new List<ProjectUpdate>();
    }
}
