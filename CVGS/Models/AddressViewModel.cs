using System.ComponentModel.DataAnnotations;

namespace CVGS.Models;
public class AddressViewModel
{
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
    [RegularExpression(@"^[A-CEGHJ-NPR-TVXY]\d[A-CEGHJ-NPR-TV-Z] \d[A-CEGHJ-NPR-TV-Z]\d$",
        ErrorMessage = "Please enter a valid Canadian postal code in the format A1A 1A1.")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; }

    public string? DeliveryInstructions { get; set; }

    public bool SameAsShippingAddress { get; set; }


    //Shipping Address
    public string? ShippingPhoneNumber { get; set; }
    public string? ShippingStreetAddress { get; set; }
    public string? ShippingAptSuite { get; set; }
    public string? ShippingCity { get; set; }
    public string? ShippingProvince { get; set; }
    [RegularExpression(@"^[A-CEGHJ-NPR-TVXY]\d[A-CEGHJ-NPR-TV-Z] \d[A-CEGHJ-NPR-TV-Z]\d$",
    ErrorMessage = "Please enter a valid Canadian postal code in the format A1A 1A1.")]
    public string? ShippingPostalCode { get; set; }
    public string? ShippingCountry { get; set; }

    public List<string> Provinces { get; set; } = new List<string>();

    public string FullAddress
    {
        get
        {
            var addressParts = new List<string>
            {
                StreetAddress,
                AptSuite,
                City,
                Province,
                PostalCode,
                Country
            };

            return string.Join(", ", addressParts.Where(part => !string.IsNullOrEmpty(part)));
        }
    }

    public string FullShippingAddress
    {
        get
        {
            var shippingAddressParts = new List<string>
            {
                ShippingStreetAddress,
                ShippingAptSuite,
                ShippingCity,
                ShippingProvince,
                ShippingPostalCode,
                ShippingCountry
            };

            return string.Join(", ", shippingAddressParts.Where(part => !string.IsNullOrEmpty(part)));
        }
    }

    public AddressViewModel()
    {
        Provinces.AddRange(new List<string>
        {
        "Alberta",
        "British Columbia",
        "Manitoba",
        "New Brunswick",
        "Newfoundland and Labrador",
        "Nova Scotia",
        "Ontario",
        "Prince Edward Island",
        "Quebec",
        "Saskatchewan",
        "Northwest Territories",
        "Nunavut",
        "Yukon"
        });
    }
}
