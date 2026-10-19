using System;
using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Website.Models
{
    /// <summary>
    /// A progress update posted against a relief project by a Foundation employee.
    /// </summary>
    public class ProjectUpdate
    {
        public int Id { get; set; }

        // Foreign key to the relief project this update belongs to.
        public int ReliefProjectId { get; set; }

        public ReliefProject? ReliefProject { get; set; }

        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Body { get; set; } = string.Empty;

        /// <summary>Email of the employee who posted the update, captured server-side.</summary>
        [MaxLength(256)]
        public string PostedBy { get; set; } = string.Empty;

        public DateTime PostedAt { get; set; }
    }
}
