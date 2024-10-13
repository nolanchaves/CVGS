using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVGS.Entities
{
    namespace CVGS.Entities
    {
        public class Preference
        {
            [Key]
            public int PreferenceId { get; set; }

            public List<string> FavouritePlatforms { get; set; }
            public List<string> FavouriteGameCategories { get; set; }
            public List<string> LanguagePreferences { get; set; }
            public List<string> AvailablePlatforms { get; set; } = new List<string>();


            [ForeignKey("User")]
            public string UserId { get; set; }
            public User User { get; set; }
        }
    }

}
