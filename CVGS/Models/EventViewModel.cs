using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public class EventViewModel
    {
        public int? EventId { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Max Registrations is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Max Registrations must be a positive number.")]
        [Display(Name = "Max Registrations")]
        public int MaxRegistrations { get; set; }
        public int CurrentRegistrations { get; set; }
        public bool IsUserRegistered { get; set; }
        public bool IsRegistrationOpen { get; set; }
    }

}
