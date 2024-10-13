using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Actual name is required.")]
        [StringLength(40, ErrorMessage = "Name cannot be longer than 40 characters.")]
        [Display(Name = "Actual Name")]
        public string ActualName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [NotFutureDate(ErrorMessage = "Birth date cannot be a future date.")]
        public DateOnly BirthDate { get; set; }

        public bool ReceivePromotionalEmails { get; set; }

        public PreferenceViewModel? Preferences { get; set; }

        public AddressViewModel? Address { get; set; }

        public List<string> FavouritePlatforms { get; set; } = new List<string>();
        public List<string> FavouriteGameCategories { get; set; } = new List<string>();
        public List<string> LanguagePreferences { get; set; } = new List<string>();


        public class NotFutureDateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is DateOnly birthDate)
                {
                    if (birthDate > DateOnly.FromDateTime(DateTime.Now))
                    {
                        return new ValidationResult("Birth date cannot be a future date.");
                    }
                }

                return ValidationResult.Success;
            }
        }
    }
}
