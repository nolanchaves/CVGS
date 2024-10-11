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

        public string Role { get; set; }

        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        [NotFutureDate(ErrorMessage = "Birth date cannot be a future date.")]
        public DateOnly? BirthDate { get; set; }

        public bool? ReceivePromotionalEmails { get; set; }

        // Foreign Key for Billing Address
        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        // Foreign Key for Shipping Address (if different from billing)
        public int? ShippingAddressId { get; set; }
        public Address? ShippingAddress { get; set; }

        public bool? SameAsShippingAddress { get; set; }

        // Navigation property for Preference
        public int? PreferenceId { get; set; }
        public Preference? Preferences { get; set; }
    }

    public class NotFutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly birthDate)
            {
                if (birthDate > DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ValidationResult("Birth date cannot be a future date.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
