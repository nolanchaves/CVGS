using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVGS.Entities
{
    public class Wishlist
    {
        [Key]
        public int WishlistId {  get; set; }

        [Required]
        [ForeignKey("GameId")]
        public int GameId {  get; set; }
        public IEnumerable<Game>? Game { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public string UserId {  get; set; }
        public IEnumerable<User>? User { get; set; }
    }
}
