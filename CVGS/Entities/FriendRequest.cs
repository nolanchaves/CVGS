using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVGS.Entities
{
    public class FriendRequest
    {
        [Key]
        public int FriendRequestId {  get; set; }
        [Required]
        [ForeignKey("PrimaryUser")]
        public string PrimaryUserId {  get; set; }// req sender
        [Required]
        [ForeignKey("SecondaryUser")]
        public string SecondaryUserId { get; set; }// req reciever

        public IEnumerable<User>? PrimaryUser {  get; set; }
        public IEnumerable<User>? SecondaryUser {  get; set; }
    }
}
