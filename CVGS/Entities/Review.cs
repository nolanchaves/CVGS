
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        //[ForeignKey("GameId")]
        public int GameId {  get; set; }
        public Game Game { get; set; }

        [Required(ErrorMessage = "Author is required")]
        //public string UserName {  get; set; }
        public string UserId { get; set; }
        public User User { get; set; } 

        [DataType(DataType.MultilineText)]
        public string? Content { get; set; }

        [Display(Name = "Rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int? Rating { get; set; }

        [Required]
        public bool Approved { get; set; } = false;
    }
}
