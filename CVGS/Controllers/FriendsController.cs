using CVGS.Entities;
using CVGS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Linq;

namespace CVGS.Controllers
{
    public class FriendsController : Controller
    {
        CvgsDbContext _context;
        UserManager<User> _userManager;

        public FriendsController(CvgsDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult FriendsList(string searchQuery)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            var _userId = _userManager.GetUserId(User);

            FriendsListViewModel vm = new FriendsListViewModel();

            List<Friends> friends = _context.Friends.Where(f => f.UserOneId == _userId || f.UserTwoId == _userId).ToList();
            vm.PendingIn = _context.FriendRequest.Where(u => u.SecondaryUserId == _userId)
                .Select(f=>new FriendsViewModel { 
                    Id=f.FriendRequestId,
                    UserId=f.PrimaryUserId,
                    UserName=_context.Users.First(u=>u.Id==f.PrimaryUserId).UserName})
                .ToList();
            vm.PendingOut = _context.FriendRequest.Where(u => u.PrimaryUserId == _userId)
                .Select(f=>new FriendsViewModel {
                    Id = f.FriendRequestId,
                    UserId =f.SecondaryUserId,
                    UserName=_context.Users.First(u=>u.Id==f.SecondaryUserId).UserName})
                .ToList();

            foreach(Friends f in friends){
                string fId = "";
                if (f.UserOneId == _userId) fId = f.UserTwoId;
                else if (f.UserTwoId == _userId) fId = f.UserOneId;

                vm.Friends.Add(new FriendsViewModel {
                    Id = f.FriendId,
                    UserId = fId, 
                    UserName = _context.Users.Where(u => u.Id == fId).FirstOrDefault().UserName 
                });
            }

            if (!searchQuery.IsNullOrEmpty())
            {
                var usersQuery = _context.Users.Where(u => u.UserName.Contains(searchQuery)).ToArray();

                foreach(User q in usersQuery)
                {
                    if (!vm.Friends.Any(f => f.UserId.Equals(q.Id)) &&
                        !vm.PendingIn.Any(f => f.UserId.Equals(q.Id)) &&
                        !vm.PendingOut.Any(f => f.UserId.Equals(q.Id)))
                    {
                        vm.Strangers.Add(new FriendsViewModel
                        {
                            UserId = q.Id,
                            UserName = q.UserName
                        });
                    }
                }
            }

            ViewBag.UserSearchQuery = searchQuery;

            return View(vm);
        }
        
        public async Task<IActionResult> AddFriend(string userId,int? reqId)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            var _userId = _userManager.GetUserId(User);

            if(!_context.Friends.Where(f => (f.UserOneId == _userId && f.UserTwoId==userId) 
            || (f.UserTwoId == _userId && f.UserOneId == userId)).Any())
            {
                var friend = new Friends();
                await _context.Friends.AddAsync(friend);
                friend.UserOneId = _userId;
                friend.UserTwoId = userId;

                await _context.SaveChangesAsync();
                Debug.WriteLine("Friends: added");
            }

            if (reqId != null)
            {
                return RedirectToAction("RemoveRequest",new { reqId = reqId });
            }

            return RedirectToAction("FriendsList");
        }
        
        public async Task<IActionResult> RemoveFriend(int friendId)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            var _userId = _userManager.GetUserId(User);

            Friends friend = _context.Friends.Where(f =>f.FriendId==friendId).FirstOrDefault();

            if (friend == null)
            {
                return RedirectToAction("FriendsList");
            }

            _context.Friends.Remove(friend);

            _context.SaveChanges();

            return RedirectToAction("FriendsList");
        }

        public async Task<IActionResult> AddRequest(string userId)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            var _userId = _userManager.GetUserId(User);

            if (!_context.Friends.Where(f => (f.UserOneId == _userId && f.UserTwoId == userId)
            || (f.UserTwoId == _userId && f.UserOneId == userId)).Any())
            {
                var req = new FriendRequest();
                await _context.FriendRequest.AddAsync(req);
                req.PrimaryUserId = _userId;
                req.SecondaryUserId = userId;

                await _context.SaveChangesAsync();
                Debug.WriteLine("Requests: added");
            }

            return RedirectToAction("FriendsList");
        }

        public async Task<IActionResult> RemoveRequest(int reqId)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            var _userId = _userManager.GetUserId(User);

            FriendRequest req = _context.FriendRequest.Where(f => f.FriendRequestId==reqId).FirstOrDefault();

            if (req == null)
            {
                return RedirectToAction("FriendsList");
            }

            _context.FriendRequest.Remove(req);

            _context.SaveChanges();

            return RedirectToAction("FriendsList");
        }
    }
}
