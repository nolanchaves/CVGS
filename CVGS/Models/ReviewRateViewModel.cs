using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public class ReviewRateViewModel
    {
        [Required]
        public int GameId {  get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rate {  get; set; }

        [StringLength(1000, ErrorMessage = "Exceeded {1} text limit")]
        public string? Review { get; set; }
    }
}
