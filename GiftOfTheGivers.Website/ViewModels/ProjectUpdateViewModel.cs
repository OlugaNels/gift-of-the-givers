using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Website.ViewModels
{
    public class ProjectUpdateViewModel
    {
        public int ReliefProjectId { get; set; }

        /// <summary>Populated by the controller for display only; never bound from the form.</summary>
        public string ProjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a headline for this update.")]
        [StringLength(150, ErrorMessage = "The headline may not exceed 150 characters.")]
        [Display(Name = "Update headline")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the update.")]
        [StringLength(4000, ErrorMessage = "The update may not exceed 4000 characters.")]
        [Display(Name = "Update")]
        public string Body { get; set; } = string.Empty;
    }
}
