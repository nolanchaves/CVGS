using Microsoft.AspNetCore.Identity;

namespace CVGS.Models
{
    public class UserRoleViewModel
    {
        public IdentityUser User { get; set; }
        public IList<string> Roles { get; set; }
    }

}
