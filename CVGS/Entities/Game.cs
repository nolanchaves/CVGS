namespace CVGS.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public int GameID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Platform { get; set; }
        public string Category { get; set; }
        public string LanguageSupport { get; set; }
        public decimal Price { get; set; }
        public double? Rating { get; set; }
        public IEnumerable<Review>? Review { get; set; }
        public string CoverImageURL { get; set; }
        public long DownloadSize { get; set; }
        public IEnumerable<Wishlist>? Wishlist { get; set; }
    }

}
