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
            "Xbox",
            "PlayStation",
            "Nintendo Switch",
            "Mobile"
        });

        AvailableGameCategories.AddRange(new List<string>
        {
            "Action",
            "Adventure",
            "RPG",
            "Simulation",
            "Strategy",
            "Sports"
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
