namespace CVGS.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public int GameID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Platform { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(100)]
        public string LanguageSupport { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, 10)]
        public double? Rating { get; set; }
        public IEnumerable<Review>? Review { get; set; }

        [StringLength(500)]
        public string CoverImageURL { get; set; }

        [Range(0, long.MaxValue)]
        public long DownloadSize { get; set; }

        public IEnumerable<Wishlist>? Wishlist { get; set; }
    }

}
