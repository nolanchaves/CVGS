namespace CVGS.Models
{
    public class FriendsListViewModel
    {
        public List<FriendsViewModel> Strangers { get; set; } = new List<FriendsViewModel>();
        public List<FriendsViewModel> Friends { get; set; } = new List<FriendsViewModel>();
        public List<FriendsViewModel> PendingIn { get; set; } = new List<FriendsViewModel>();
        public List<FriendsViewModel> PendingOut { get; set; } = new List<FriendsViewModel>();
    }
}
