public class PreferenceViewModel
{
    public List<string> FavouritePlatforms { get; set; } = new List<string>();
    public List<string> FavouriteGameCategories { get; set; } = new List<string>();
    public List<string> LanguagePreferences { get; set; } = new List<string>();

   
    public List<string> AvailablePlatforms { get; set; } = new List<string>();
    public List<string> AvailableGameCategories { get; set; } = new List<string>();
    public List<string> AvailableLanguages { get; set; } = new List<string>();

    public PreferenceViewModel()
    {
        AvailablePlatforms.AddRange(new List<string>
        {
            "PC",
            "Xbox Series S/X",
            "Xbox One",
            "PlayStation 5",
            "PlayStation 4",
            "Nintendo Switch"
        });

        AvailableGameCategories.AddRange(new List<string>
        {
            "Action",
            "Adventure",
            "Multiplayer",
            "RPG",
            "Simulation",
            "Strategy",
            "Sports",
            "Shooter"
        });

        AvailableLanguages.AddRange(new List<string>
        {
            "English",
            "Spanish",
            "French",
            "German",
            "Chinese",
            "Japanese",
            "Russian"
        });
    }
}
