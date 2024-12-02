using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVGS.Entities
{
    public class Friends
    {
        [Key]
        public int FriendId { get; set; }
        [Required]
        [ForeignKey("UserOne")]
        public string UserOneId { get; set; }
        [Required]
        [ForeignKey("UserTwo")]
        public string UserTwoId { get; set; }

        public IEnumerable<User>? UserOne {  get; set; }
        public IEnumerable<User>? UserTwo {  get; set; }
    }
}
