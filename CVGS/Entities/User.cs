using CVGS.Entities.CVGS.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Full Name can only contain letters and spaces.")]
        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public DateOnly? BirthDate { get; set; }

        public bool? ReceivePromotionalEmails { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public int? ShippingAddressId { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }

        public int? PreferenceId { get; set; }
        public Preference? Preferences { get; set; }
    }
}
