namespace CVGS.Models
{
    public class GameViewModel
    {
        public int GameID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Platform { get; set; }
        public string Category { get; set; }
        public string LanguageSupport { get; set; }
        public decimal Price { get; set; }
        public float Rating { get; set; }
        public string CoverImageURL { get; set; }
        public long DownloadSize { get; set; }
    }

}
