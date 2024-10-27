using CVGS.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVGS.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Developer is required")]
        [StringLength(100, ErrorMessage = "Developer cannot exceed 100 characters")]
        public string Developer { get; set; }

        [Required(ErrorMessage = "Publisher is required")]
        [StringLength(100, ErrorMessage = "Publisher cannot exceed 100 characters")]
        public string Publisher { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50, ErrorMessage = "Genre cannot exceed 50 characters")]
        public string Genre { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Overall Rating")]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
        public float OverallRating { get; set; }

        [Display(Name = "Graphics Rating")]
        [Range(1, 10, ErrorMessage = "Graphics rating must be between 1 and 10")]
        public float GraphicsRating { get; set; }

        [Display(Name = "Sound Rating")]
        [Range(1, 10, ErrorMessage = "Sound rating must be between 1 and 10")]
        public float SoundRating { get; set; }

        [Display(Name = "Gameplay Rating")]
        [Range(1, 10, ErrorMessage = "Gameplay rating must be between 1 and 10")]
        public float GameplayRating { get; set; }

        [InverseProperty("Videogame")]
        public ICollection<Review> Reviews { get; set; }
    }
}

