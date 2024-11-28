using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public class GameViewModel
    {
        public int GameID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Platform { get; set; }
        public string Category { get; set; }

        [Display(Name = "Language")]
        public string LanguageSupport { get; set; }
        public decimal Price { get; set; }
        public float Rating { get; set; }
        public string? CoverImageURL { get; set; }
        public IFormFile? CoverImage { get; set; }  // For handling file uploads
        public long DownloadSize { get; set; }
        public List<ReviewDetailViewModel>? Reviews { get; set; }
        public ReviewDetailViewModel? UserReview {  get; set; }
        public PreferenceViewModel? PreferenceViewModel { get; set; }
    }

}
