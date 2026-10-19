using System;
using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Website.ViewModels
{
    public class ReliefProjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a project name.")]
        [StringLength(150, ErrorMessage = "The project name may not exceed 150 characters.")]
        [Display(Name = "Project name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please describe the relief operation.")]
        [StringLength(1000, ErrorMessage = "The description may not exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the location of the operation.")]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a status.")]
        public string Status { get; set; } = "Planned";

        [Range(0, 100000000, ErrorMessage = "The funding goal must be zero or greater.")]
        [Display(Name = "Funding goal")]
        public decimal GoalAmount { get; set; }

        [Range(0, 100000000, ErrorMessage = "The amount raised must be zero or greater.")]
        [Display(Name = "Amount raised")]
        public decimal RaisedAmount { get; set; }

        [Required(ErrorMessage = "Please enter a start date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        public DateTime? EndDate { get; set; }
    }
}
