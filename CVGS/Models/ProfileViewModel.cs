using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Actual name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string ActualName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }

        public bool ReceivePromotionalEmails { get; set; }

        public PreferenceViewModel? Preferences { get; set; }

        public AddressViewModel? Address { get; set; }

        public List<string> FavoritePlatforms { get; set; } = new List<string>();
        public List<string> FavoriteGameCategories { get; set; } = new List<string>();
        public List<string> LanguagePreferences { get; set; } = new List<string>();
    }
}
