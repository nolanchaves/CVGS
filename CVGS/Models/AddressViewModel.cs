using System.ComponentModel.DataAnnotations;

namespace CVGS.Models;
public class AddressViewModel
{
    [Required(ErrorMessage = "Full name is required.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Street address is required.")]
    public string StreetAddress { get; set; }

    public string? AptSuite { get; set; } // Optional

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Province is required.")]
    public string Province { get; set; }

    [Required(ErrorMessage = "Postal code is required.")]
    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string? DeliveryInstructions { get; set; } // Optional

    public bool SameAsShippingAddress { get; set; } // Indicates if mailing and shipping addresses are the same
}
