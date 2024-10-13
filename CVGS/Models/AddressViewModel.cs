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

    public string? AptSuite { get; set; }

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Province is required.")]
    public string Province { get; set; }

    [Required(ErrorMessage = "Postal code is required.")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; }

    public string? DeliveryInstructions { get; set; }

    public bool SameAsShippingAddress { get; set; }


    //Shipping Address
    public string? ShippingFullName { get; set; }
    public string? ShippingPhoneNumber { get; set; }
    public string? ShippingStreetAddress { get; set; }
    public string? ShippingAptSuite { get; set; }
    public string? ShippingCity { get; set; }
    public string? ShippingProvince { get; set; }
    public string? ShippingPostalCode { get; set; }
    public string? ShippingCountry { get; set; }
}
